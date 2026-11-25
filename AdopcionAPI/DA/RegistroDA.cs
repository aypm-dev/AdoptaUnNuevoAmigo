using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Consultas;
using Abstracciones.Especies;
using Abstracciones.Razas;
using Abstracciones.Seguimientos;
using Abstracciones.Solicitudes;
using Abstracciones.TiposVacunas;
using Abstracciones.TiposViviendas;
using Abstracciones.Vacunas;
using Abstracciones.Veterinarias;
using Abstracciones.VisitasDomiciliarias;
using DA.Adoptantes;
using DA.Animales;
using DA.Comunes;
using DA.Consultas;
using DA.Contexto.AdopcionContexto;
using DA.Contexto.UsuarioContexto;
using DA.Especies;
using DA.Razas;
using DA.Seguimientos;
using DA.Solicitudes;
using DA.TiposVacunas;
using DA.TiposViviendas;
using DA.Vacunas;
using DA.Veterinarias;
using DA.VisitasDomiciliarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DA;

/// <summary>
/// La capa DA registra sus propias dependencias. El API solo llama a AddDatos(...).
/// </summary>
public static class RegistroDA
{
    public static IServiceCollection AddDatos(this IServiceCollection servicios, IConfiguration configuracion)
    {
        var cadenaConexion = configuracion.GetConnectionString("AdopcionDB")
            ?? configuracion.GetConnectionString("Adopcion")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'Adopcion' o 'AdopcionDB' en la configuración.");

        // Configuración de Entity Framework Core con reintentos para resiliencia (Azure SQL Serverless / LocalDB)
        servicios.AddDbContext<AdopcionContexto>(opciones =>
            opciones.UseSqlServer(cadenaConexion, sql => sql.EnableRetryOnFailure(maxRetryCount: 5)));

        servicios.AddDbContext<UsuarioContexto>(opciones =>
            opciones.UseSqlServer(cadenaConexion, sql => sql.EnableRetryOnFailure(maxRetryCount: 5)));

        // Registro de repositorios DA con ciclo de vida Scoped
        servicios.AddScoped<IAdoptanteDA, AdoptanteDA>();
        servicios.AddScoped<IAnimalDA, AnimalDA>();
        servicios.AddScoped<IConsultaDA, ConsultaDA>();
        servicios.AddScoped<IEspecieDA, EspecieDA>();
        servicios.AddScoped<IRazaDA, RazaDA>();
        servicios.AddScoped<ISeguimientoDA, SeguimientoDA>();
        servicios.AddScoped<ISolicitudDA, SolicitudDA>();
        servicios.AddScoped<ITipoVacunaDA, TipoVacunaDA>();
        servicios.AddScoped<ITipoViviendaDA, TipoViviendaDA>();
        servicios.AddScoped<IVacunacionDA, VacunacionDA>();
        servicios.AddScoped<IVeterinariaDA, VeterinariaDA>();
        servicios.AddScoped<IVisitaDA, VisitaDA>();
        servicios.AddScoped<IVoluntarioDA, VoluntarioDA>();

        // Chequeo de salud de base de datos
        servicios.AddScoped<ISaludBaseDatos, SaludBaseDatos>();

        return servicios;
    }

    /// <summary>
    /// Aplica las migraciones pendientes al arrancar (cuando Base:MigrarAlIniciar = true).
    /// Imprime el servidor y nombre de la base de datos sin exponer contraseñas ni credenciales.
    /// </summary>
    public static async Task AplicarMigracionesAsync(this IServiceProvider servicios, CancellationToken ct = default)
    {
        using var alcance = servicios.CreateScope();
        var bitacora = alcance.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migraciones");

        await MigrarContextoAsync<AdopcionContexto>(alcance.ServiceProvider, bitacora, ct);
        await MigrarContextoAsync<UsuarioContexto>(alcance.ServiceProvider, bitacora, ct);
    }

    private static async Task MigrarContextoAsync<TContext>(
        IServiceProvider proveedor,
        ILogger bitacora,
        CancellationToken ct) where TContext : DbContext
    {
        var contexto = proveedor.GetRequiredService<TContext>();
        var nombreContexto = typeof(TContext).Name;
        var migracionesDefinidas = contexto.Database.GetMigrations();

        if (!migracionesDefinidas.Any())
        {
            bitacora.LogInformation("[{Contexto}] Sin migraciones definidas; se omite MigrateAsync.", nombreContexto);
            return;
        }

        var conexion = contexto.Database.GetDbConnection();
        bitacora.LogInformation(
            "[{Contexto}] Verificando migraciones -> Servidor: {Servidor} | Base: {Base}",
            nombreContexto,
            conexion.DataSource,
            conexion.Database);

        var pendientes = (await contexto.Database.GetPendingMigrationsAsync(ct)).ToList();
        if (pendientes.Count == 0)
        {
            bitacora.LogInformation("[{Contexto}] Sin migraciones pendientes.", nombreContexto);
        }
        else
        {
            bitacora.LogInformation(
                "[{Contexto}] Aplicando migraciones pendientes: {Lista}",
                nombreContexto,
                string.Join(", ", pendientes));
        }

        await contexto.Database.MigrateAsync(ct);
    }
}
