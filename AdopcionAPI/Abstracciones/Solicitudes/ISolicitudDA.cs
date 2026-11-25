using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Seguimientos;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Solicitudes.SolicitudAdopcionDtos;

namespace Abstracciones.Solicitudes
{
    public interface ISolicitudDA
    {

        Task<ResultadoPaginado<SolicitudAdopcion>> BuscarAsync(SolicitudFiltro filtro, CancellationToken ct);
        Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id, CancellationToken ct);

        /// <summary>Inserta la solicitud Y pone el animal en EnProceso, en un único SaveChanges.</summary>
        Task<SolicitudAdopcion> AgregarAsync(SolicitudAdopcion solicitud, CancellationToken ct);

        /// <summary>Cambia el estado de la solicitud y del animal atómicamente.</summary>
        Task<bool> CambiarEstadoAsync(int id, EstadoSolicitud nuevo, EstadoAnimal estadoAnimal,
            DateTime? fechaResolucion, IReadOnlyList<SeguimientoPostAdopcion>? seguimientos, CancellationToken ct);
        Task<bool> ExisteActivaParaAnimalAsync(int animalId, CancellationToken ct);
        Task<DateTime?> UltimoRechazoAsync(int adoptanteId, CancellationToken ct);
        Task<int> ContarAnimalesEnSeguimientoAsync(int adoptanteId, CancellationToken ct);

    }
}
