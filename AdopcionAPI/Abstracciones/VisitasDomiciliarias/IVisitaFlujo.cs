using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace Abstracciones.VisitasDomiciliarias
{
    public interface IVisitaFlujo
    {

        Task<ResultadoPaginado<VisitaDto>> ListarAsync(VisitaFiltro f, CancellationToken ct);
        Task<VisitaDto> ObtenerAsync(int id, CancellationToken ct);
        Task<VisitaDto> CrearAsync(VisitaCrearDTO dto, CancellationToken ct);
        Task RegistrarResultadoAsync(int id, VisitaResultadoDTO dto, CancellationToken ct);


    }
}
