using Abstracciones.Animales;
using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace Abstracciones.Seguimientos
{
    public interface ISeguimientoDA
    {
        Task<ResultadoPaginado<SeguimientoPostAdopcion>> BuscarAsync(SeguimientoFiltro f, CancellationToken ct);
        Task<SeguimientoPostAdopcion?> ObtenerPorIdAsync(int id, CancellationToken ct);

        /// <summary>Registra el resultado y, si cierra la adopción (hito 90), cambia el animal y deja el evento: todo atómico.</summary>
        Task<bool> RegistrarResultadoAsync(int id, EstadoSeguimiento estado, string? observaciones, DateTime fecha,
            EstadoAnimal? nuevoEstadoAnimal, EventoAdopcion? evento, CancellationToken ct);

        Task<int> MarcarVencidosYCrearAlertasAsync(DateTime ahora, CancellationToken ct);
        Task<IReadOnlyList<AlertaSeguimiento>> ObtenerAlertasSinEscalarAsync(CancellationToken ct);
        Task MarcarEscaladaAsync(int idAlerta, DateTime fecha, CancellationToken ct);
        Task<IReadOnlyList<AlertaSeguimiento>> ListarAlertasAsync(CancellationToken ct);


    }
}
