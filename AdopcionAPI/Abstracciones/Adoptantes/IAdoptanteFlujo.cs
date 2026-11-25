using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace Abstracciones.Adoptantes
{
    public interface IAdoptanteFlujo
    {

        Task<ResultadoPaginado<AdoptanteDto>> ListarAsync(AdoptanteFiltro filtro, CancellationToken ct);
        Task<AdoptanteDto> ObtenerAsync(int id, CancellationToken ct);
        Task<AdoptanteDto> CrearAsync(AdoptanteCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, AdoptanteActualizarDTO dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
