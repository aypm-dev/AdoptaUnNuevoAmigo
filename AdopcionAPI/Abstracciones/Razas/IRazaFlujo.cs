using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Razas.RazaDtos;

namespace Abstracciones.Razas
{
    public interface IRazaFlujo
    {

        Task<ResultadoPaginado<RazaDto>> ListarAsync(RazaFiltro filtro, CancellationToken ct);
        Task<RazaDto> CrearAsync(RazaCrearDTO dto, CancellationToken ct);
        Task<RazaDto> ObtenerAsync(int id, CancellationToken ct);


    }
}
