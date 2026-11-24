using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Consultas.ConsultaDtos;

namespace Abstracciones.Consultas
{
    public interface IConsultaFlujo
    {

        Task<ResultadoPaginado<ConsultaDto>> ListarAsync(ConsultaFiltro f, CancellationToken ct);
        Task<ConsultaDto> ObtenerAsync(int id, CancellationToken ct);
        Task<ConsultaDto> CrearAsync(ConsultaCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, ConsultaActualizarDTO dto, CancellationToken ct);


    }
}
