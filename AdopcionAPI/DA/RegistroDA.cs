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
}
