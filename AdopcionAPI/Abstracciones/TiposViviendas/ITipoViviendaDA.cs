using Abstracciones.Comunes;
using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace Abstracciones.TiposViviendas
{
    public interface ITipoViviendaDA
    {

        Task<ResultadoPaginado<TipoVivienda>> BuscarAsync(TipoViviendaFiltro filtro, CancellationToken ct);
        Task<TipoVivienda?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<TipoVivienda> AgregarAsync(TipoVivienda tipoVivienda, CancellationToken ct);

        Task<bool> ActualizarAsync(TipoVivienda datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);



    }
}
