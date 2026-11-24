using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;

namespace Abstracciones.Especies
{
    public interface IEspecieFlujo
    {

        Task<ResultadoPaginado<EspecieDto>> ListarAsync(EspecieFiltro filtro, CancellationToken ct);
        Task<EspecieDto> ObtenerAsync(int id, CancellationToken ct);
        Task<EspecieDto> CrearAsync(EspecieCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, EspecieActualizarDTO dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
