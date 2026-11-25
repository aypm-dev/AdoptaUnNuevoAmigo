using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Animales.AnimalDtos;
using static Abstracciones.Especies.IEspecieDtos;

namespace Abstracciones.Animales
{
    public interface IAnimalFlujo
    {

        Task<ResultadoPaginado<AnimalDto>> ListarAsync(AnimalFiltro filtro, CancellationToken ct);
        Task<AnimalDto> ObtenerAsync(int id, CancellationToken ct);
        Task<AnimalDto> CrearAsync(AnimalCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, AnimalActualizarDTO dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
