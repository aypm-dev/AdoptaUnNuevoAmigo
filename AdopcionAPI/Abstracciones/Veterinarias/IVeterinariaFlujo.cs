using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;
using static Abstracciones.Veterinarias.VeterinariaDtos;

namespace Abstracciones.Veterinarias
{
    public interface IVeterinariaFlujo
    {

        Task<ResultadoPaginado<VeterinariaDto>> ListarAsync(VeterinariaFiltro filtro, CancellationToken ct);
        Task<VeterinariaDto> ObtenerAsync(int id, CancellationToken ct);
        Task<VeterinariaDto> CrearAsync(VeterinariaCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, VeterinariaActualizarDTO    dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
