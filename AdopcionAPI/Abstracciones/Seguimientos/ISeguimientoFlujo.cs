using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace Abstracciones.Seguimientos
{
    public interface ISeguimientoFlujo
    {

        Task<ResultadoPaginado<SeguimientoDto>> ListarAsync(SeguimientoFiltro f, CancellationToken ct);
        Task<SeguimientoDto> ObtenerAsync(int id, CancellationToken ct);
        Task RegistrarResultadoAsync(int id, SeguimientoResultadoDTO dto, CancellationToken ct);
        Task<IReadOnlyList<AlertaDto>> ListarAlertasAsync(CancellationToken ct);

    }

    public interface IVencimientosSeguimientoFlujo { Task<int> ProcesarAsync(CancellationToken ct); }

    public interface INotificadorCoordinador { Task NotificarAsync(AlertaSeguimiento alerta, CancellationToken ct); }

}
