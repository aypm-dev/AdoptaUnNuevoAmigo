using Abstracciones.Adoptantes;
using Abstracciones.Animales;
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
using Flujo.Adoptantes;
using Flujo.Animales;
using Flujo.Consultas;
using Flujo.Especies;
using Flujo.Razas;
using Flujo.Seguimientos;
using Flujo.Solicitudes;
using Flujo.TiposVacunas;
using Flujo.TiposViviendas;
using Flujo.VacunacionFlujo;
using Flujo.Veterinarias;
using Flujo.VisitasDomiciliarias;
using Microsoft.Extensions.DependencyInjection;

namespace Flujo;

/// <summary>
/// La capa Flujo registra sus propios servicios. Es Scoped porque depende de los DA, que son Scoped.
/// </summary>
public static class RegistroFlujo
{
    public static IServiceCollection AddFlujo(this IServiceCollection servicios)
    {
        servicios.AddSingleton(TimeProvider.System);
        servicios.AddOptions<SeguimientoOpciones>();
        servicios.AddOptions<ReglasAdopcionOpciones>();

        // Registro de servicios de Flujo con ciclo de vida Scoped
        servicios.AddScoped<IAdoptanteFlujo, AdoptanteFlujo>();
        servicios.AddScoped<IAnimalFlujo, AnimalFlujo>();
        servicios.AddScoped<IConsultaFlujo, ConsultaFlujo>();
        servicios.AddScoped<IEspecieFlujo, EspecieFlujo>();
        servicios.AddScoped<IRazaFlujo, RazaFlujo>();
        servicios.AddScoped<ISeguimientoFlujo, SeguimientoFlujo>();
        servicios.AddScoped<IVencimientosSeguimientoFlujo, VencimientosSeguimientoFlujo>();
        servicios.AddScoped<INotificadorCoordinador, NotificadorCoordinadorLog>();
        servicios.AddScoped<ISolicitudFlujo, SolicitudFlujo>();
        servicios.AddScoped<ITipoVacunaFlujo, TipoVacunaFlujo>();
        servicios.AddScoped<ITipoViviendaFlujo, TipoViviendaFlujo>();
        servicios.AddScoped<IVacunacionFlujo, Flujo.VacunacionFlujo.VacunacionFlujo>();
        servicios.AddScoped<IVeterinariaFlujo, VeterinariaFlujo>();
        servicios.AddScoped<IVisitaFlujo, VisitaFlujo>();

        // Tarea en segundo plano para la regla de vencimiento de seguimientos
        servicios.AddHostedService<VencimientosSeguimientoWorker>();

        return servicios;
    }
}
