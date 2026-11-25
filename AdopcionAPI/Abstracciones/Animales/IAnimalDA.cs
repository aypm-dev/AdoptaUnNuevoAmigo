using Abstracciones.Comunes;
using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static Abstracciones.Animales.AnimalDtos;

namespace Abstracciones.Animales
{
    public interface IAnimalDA
    {

        Task<ResultadoPaginado<Animal>> BuscarAsync(AnimalFiltro filtro, CancellationToken ct);

        Task<Animal?> ObtenerPorIdAsync(int id, CancellationToken ct);

        Task<Animal> AgregarAsync(Animal animal, CancellationToken ct);

        Task<bool> ActualizarAsync(Animal datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);
    }
}
