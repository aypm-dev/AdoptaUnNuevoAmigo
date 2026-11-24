using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.TiposVacunas;
using DA;
using Flujo.Especies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.TiposVacunas
{
    public class TipoVacunaFlujo : ITipoVacunaFlujo
    {

        private readonly ITipoVacunaDA _tipoVacunaDA;
        
        public TipoVacunaFlujo(ITipoVacunaDA tipoVacunaDA)
        {
            _tipoVacunaDA = tipoVacunaDA;
        }

        public async Task ActualizarAsync(int id, TipoVacunaDtos.TipoVacunaActualizarDTO dto, CancellationToken ct)
        {
            _ = await _tipoVacunaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Tipo de Vacuna", id);

            // La descripción puede repetirse solo si pertenece a la MISMA especie que se está editando.
            var actualizado = await _tipoVacunaDA.ActualizarAsync(new TipoVacuna
            {
                IdTipoVacuna = id,
                Nombre = dto.Nombre.Trim(),
                Obligatoria = dto.Obligatoria,
                Activo = dto.Activo
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Tipo de Vacuna", id);
        }

        public async Task<TipoVacunaDtos.TipoVacunaDto> CrearAsync(TipoVacunaDtos.TipoVacunaCrearDTO dto, CancellationToken ct)
        {
            var tipoVacuna = new TipoVacuna
            {
                Nombre = dto.Nombre.Trim(),
                Obligatoria = dto.Obligatoria,
                Activo = dto.Activo,
            };

            await _tipoVacunaDA.AgregarAsync(tipoVacuna, ct);
            return TipoVacunaMapeador.ADto(tipoVacuna);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _tipoVacunaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Tipo de Vacuna", id);

            if (!await _tipoVacunaDA.EliminarAsync(id, ct))
                throw new NoEncontradoException("Tipo de Vacuna", id);
        }

        public async Task<ResultadoPaginado<TipoVacunaDtos.TipoVacunaDto>> ListarAsync(TipoVacunaDtos.TipoVacunaFiltro filtro, CancellationToken ct)
        {
            var pagina = await _tipoVacunaDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<TipoVacunaDtos.TipoVacunaDto>
            {
                Items = pagina.Items.Select(TipoVacunaMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<TipoVacunaDtos.TipoVacunaDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var tipoVacuna = await _tipoVacunaDA.ObtenerPorIdAsync(id, ct)
           ?? throw new NoEncontradoException("Tipo de Vacuna", id);
            return TipoVacunaMapeador.ADto(tipoVacuna);
        }
    }
}
