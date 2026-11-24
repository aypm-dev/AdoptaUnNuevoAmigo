using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace Abstracciones.TiposViviendas
{
    public interface ITipoViviendaFlujo
    {

        Task<ResultadoPaginado<TipoViviendaDto>> ListarAsync(TipoViviendaFiltro filtro, CancellationToken ct);
        Task<TipoViviendaDto> ObtenerAsync(int id, CancellationToken ct);
        Task<TipoViviendaDto> CrearAsync(TipoViviendaCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, TipoViviendaActualizarDTO dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
