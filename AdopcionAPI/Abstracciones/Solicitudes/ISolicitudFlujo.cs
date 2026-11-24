using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Solicitudes.SolicitudAdopcionDtos;

namespace Abstracciones.Solicitudes
{
    public interface ISolicitudFlujo
    {

        Task<ResultadoPaginado<SolicitudDto>> ListarAsync(SolicitudFiltro filtro, CancellationToken ct);
        Task<SolicitudDto> ObtenerAsync(int id, CancellationToken ct);
        Task<SolicitudDto> CrearAsync(SolicitudCrearDTO dto, CancellationToken ct);
        Task CambiarEstadoAsync(int id, EstadoSolicitud nuevo, CancellationToken ct);

    }
}
