using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Modelos;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Especies
{
    public class EspecieFlujo : IEspecieFlujo
    {

        private readonly IEspecieDA _especieDA;
        private readonly TimeProvider _reloj;

        public EspecieFlujo(IEspecieDA especieDA, TimeProvider reloj)
        {
            _especieDA = especieDA;
            _reloj = reloj;
        }

        public async Task ActualizarAsync(int id, IEspecieDtos.EspecieActualizarDTO dto, CancellationToken ct)
        {
            _ = await _especieDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Especie", id);

            // La descripción puede repetirse solo si pertenece a la MISMA especie que se está editando.
            var actualizado = await _especieDA.ActualizarAsync(new Especie
            {
                IdEspecie = id,
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Activo = dto.Activo
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Especie", id);
        }

        public async Task<IEspecieDtos.EspecieDto> CrearAsync(IEspecieDtos.EspecieCrearDTO dto, CancellationToken ct)
        {

            var especie = new Especie
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Activo = dto.Activo,
                FechaRegistro = _reloj.GetUtcNow().UtcDateTime
            };

            await _especieDA.AgregarAsync(especie, ct);
            return EspecieMapeador.ADto(especie);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _especieDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Especie", id);

            if (!await _especieDA.EliminarAsync(id, ct))
                throw new NoEncontradoException("Especie", id);
        }

        public async Task<ResultadoPaginado<IEspecieDtos.EspecieDto>> ListarAsync(IEspecieDtos.EspecieFiltro filtro, CancellationToken ct)
        {
            var pagina = await _especieDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<IEspecieDtos.EspecieDto>
            {
                Items = pagina.Items.Select(EspecieMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<IEspecieDtos.EspecieDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var especie = await _especieDA.ObtenerPorIdAsync(id, ct)
            ?? throw new NoEncontradoException("Especie", id);
            return EspecieMapeador.ADto(especie);
        }
    }
}
