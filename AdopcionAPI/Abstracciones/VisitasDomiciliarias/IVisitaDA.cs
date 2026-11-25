using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace Abstracciones.VisitasDomiciliarias
{
    public interface IVisitaDA
    {

        Task<ResultadoPaginado<VisitaDomiciliaria>> BuscarAsync(VisitaFiltro f, CancellationToken ct);
        Task<VisitaDomiciliaria?> ObtenerPorIdAsync(int id, CancellationToken ct);
        /// <summary>Inserta la visita y, si la solicitud estaba EnRevision, la pasa a VisitaProgramada (un solo SaveChanges).</summary>
        Task<VisitaDomiciliaria> AgregarAsync(VisitaDomiciliaria v, CancellationToken ct);
        Task<bool> RegistrarResultadoAsync(int id, ResultadoVisita resultado, string? observaciones, CancellationToken ct);
        Task<bool> ExistePendienteAsync(int solicitudId, CancellationToken ct);
        /// <summary>Resultado de la última visita ya resuelta (null si no hay ninguna).</summary>
        Task<ResultadoVisita?> UltimoResultadoAsync(int solicitudId, CancellationToken ct);

    }
}
