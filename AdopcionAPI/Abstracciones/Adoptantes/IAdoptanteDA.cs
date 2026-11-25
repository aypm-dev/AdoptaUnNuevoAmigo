using Abstracciones.Animales;
using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace Abstracciones.Adoptantes
{
    public interface IAdoptanteDA
    {

        Task<ResultadoPaginado<Adoptante>> BuscarAsync(AdoptanteFiltro filtro, CancellationToken ct);

        Task<Adoptante?> ObtenerPorIdAsync(int id, CancellationToken ct);

        Task<Adoptante> AgregarAsync(Adoptante adoptante, CancellationToken ct);

        Task<bool> ActualizarAsync(Adoptante datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);

    }
}
