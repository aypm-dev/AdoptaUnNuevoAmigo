using Abstracciones.Comunes;
using Abstracciones.TiposViviendas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;
using static Abstracciones.Veterinarias.VeterinariaDtos;

namespace Abstracciones.Veterinarias
{
    public interface IVeterinariaDA
    {

        Task<ResultadoPaginado<Veterinaria>> BuscarAsync(VeterinariaFiltro filtro, CancellationToken ct);
        Task<Veterinaria?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Veterinaria> AgregarAsync(Veterinaria veterinaria, CancellationToken ct);

        Task<bool> ActualizarAsync(Veterinaria datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);




    }

}
