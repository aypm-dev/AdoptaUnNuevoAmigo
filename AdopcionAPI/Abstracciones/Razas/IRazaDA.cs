using Abstracciones.Comunes;
using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.Razas.RazaDtos;

namespace Abstracciones.Razas
{
    public interface IRazaDA
    {

        Task<ResultadoPaginado<Raza>> BuscarAsync(RazaFiltro filtro, CancellationToken ct);
        Task<Raza> AgregarAsync(Raza raza, CancellationToken ct);
        Task<Raza?> ObtenerPorIdAsync(int id, CancellationToken ct);


    }
}
