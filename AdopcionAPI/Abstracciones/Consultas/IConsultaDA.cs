using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Consultas.ConsultaDtos;

namespace Abstracciones.Consultas
{
    public interface IConsultaDA
    {

        Task<ResultadoPaginado<ConsultaVeterinaria>> BuscarAsync(ConsultaFiltro f, CancellationToken ct);
        Task<ConsultaVeterinaria?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<ConsultaVeterinaria> AgregarAsync(ConsultaVeterinaria c, CancellationToken ct);
        Task<bool> ActualizarAsync(ConsultaVeterinaria datos, CancellationToken ct);

    }
}
