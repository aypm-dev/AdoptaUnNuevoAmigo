using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace Abstracciones.Vacunas
{
    public interface IVacunacionFlujo
    {

        Task<ResultadoPaginado<VacunacionDto>> ListarAsync(VacunacionFiltro f, CancellationToken ct);
        Task<VacunacionDto> ObtenerAsync(int id, CancellationToken ct);
        Task<VacunacionDto> CrearAsync(VacunacionCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, VacunacionActualizarDTO dto, CancellationToken ct);

    }
}
