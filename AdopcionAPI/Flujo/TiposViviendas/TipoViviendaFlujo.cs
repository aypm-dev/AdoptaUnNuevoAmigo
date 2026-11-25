using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.TiposViviendas;
using DA;
using Flujo.Especies;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace Flujo.TiposViviendas
{
    public class TipoViviendaFlujo : ITipoViviendaFlujo
    {
        private readonly ITipoViviendaDA _tipoViviendaDA;
        private readonly TimeProvider _reloj;

        public TipoViviendaFlujo(ITipoViviendaDA tipoViviendaDA, TimeProvider reloj)
        {
            _tipoViviendaDA = tipoViviendaDA;
            _reloj = reloj;
        }

        public async Task ActualizarAsync(int id, TipoViviendaDtos.TipoViviendaActualizarDTO dto, CancellationToken ct)
        {
            _ = await _tipoViviendaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("TipoVivienda", id);

            // La descripción puede repetirse solo si pertenece a la MISMA especie que se está editando.
            var actualizado = await _tipoViviendaDA.ActualizarAsync(new TipoVivienda
            {
                IdTipoVivienda  = id,
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Activo = dto.Activo
            }, ct);

            if (!actualizado) throw new NoEncontradoException("TipoVivienda", id);
        }

        public async Task<TipoViviendaDto> CrearAsync(TipoViviendaCrearDTO dto, CancellationToken ct)
        {
            var tipoVivienda = new TipoVivienda
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Activo = dto.Activo,
                FechaRegistro = _reloj.GetUtcNow().UtcDateTime
            };

            await _tipoViviendaDA.AgregarAsync(tipoVivienda, ct);
            return TipoViviendaMapeador.ADto(tipoVivienda);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _tipoViviendaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("TipoVivienda", id);

            if (!await _tipoViviendaDA.EliminarAsync(id, ct))
                throw new NoEncontradoException("TipoVivienda", id);
        }

        public async Task<ResultadoPaginado<TipoViviendaDto>> ListarAsync(TipoViviendaFiltro filtro, CancellationToken ct)
        {
            var pagina = await _tipoViviendaDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<TipoViviendaDto>
            {
                Items = pagina.Items.Select(TipoViviendaMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<TipoViviendaDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var tipoVivienda = await _tipoViviendaDA.ObtenerPorIdAsync(id, ct)
           ?? throw new NoEncontradoException("TipoVivienda", id);
            return TipoViviendaMapeador.ADto(tipoVivienda);
        }
    }
}
