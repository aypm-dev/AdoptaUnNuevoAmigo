using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using DA.Animales;
using Flujo.Animales;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace Flujo.Adoptantes
{
    public class AdoptanteFlujo : IAdoptanteFlujo
    {
        private readonly IAdoptanteDA _adoptanteDA;

        private readonly TimeProvider _reloj;

       public AdoptanteFlujo(IAdoptanteDA adoptanteDA, TimeProvider reloj)
        {
            _adoptanteDA = adoptanteDA;
            _reloj = reloj; 
        }

        public async Task ActualizarAsync(int id, AdoptanteDtos.AdoptanteActualizarDTO dto, CancellationToken ct)
        {

            if (await _adoptanteDA.ObtenerPorIdAsync(dto.TipoViviendaId, ct) is null)
                throw new ValidacionNegocioException($"El tipo de vivienda {dto.TipoViviendaId} no existe.");

            _ = await _adoptanteDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Adoptante", id);

            // La descripción puede repetirse solo si pertenece a la MISMA especie que se está editando.
            var actualizado = await _adoptanteDA.ActualizarAsync(new Adoptante
            {
                IdAdoptante = id,
                Identificacion = dto.Identificacion.Trim(),
                Nombre = dto.Nombre.Trim(),
                Contacto = dto.Contacto.Trim(),
                OtrasMascotas = dto.OtrasMascotas.Trim(),
                Activo = dto.Activo,
                FechaRegistro = dto.FechaRegistro,
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Adoptante", id);
        }

        public async Task<AdoptanteDtos.AdoptanteDto> CrearAsync(AdoptanteDtos.AdoptanteCrearDTO dto, CancellationToken ct)
        {
            if (await _adoptanteDA.ObtenerPorIdAsync(dto.TipoViviendaId, ct) is null)
                throw new ValidacionNegocioException($"El Tipo de vivienda {dto.TipoViviendaId} no existe.");

            var ahora = _reloj.GetUtcNow().UtcDateTime;

            var adoptante = new Adoptante
            {
                // El código lo genera el sistema; el cliente HTTP nunca lo envía.
                Identificacion = dto.Identificacion.Trim(),
                Nombre = dto.Nombre.Trim(),
                Contacto = dto.Contacto.Trim(),
                OtrasMascotas = dto.OtrasMascotas.Trim(),
                Activo = dto.Activo,
                FechaRegistro = ahora

            };

            await _adoptanteDA.AgregarAsync(adoptante, ct);
            return await ObtenerAsync(adoptante.IdAdoptante, ct);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _adoptanteDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Adoptante", id);

            if (!await _adoptanteDA.EliminarAsync(id, ct))
                throw new NoEncontradoException("Adoptante", id);
        }

        public async Task<ResultadoPaginado<AdoptanteDtos.AdoptanteDto>> ListarAsync(AdoptanteDtos.AdoptanteFiltro filtro, CancellationToken ct)
        {
            var pagina = await _adoptanteDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<AdoptanteDto>
            {
                Items = pagina.Items.Select(AdoptanteMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<AdoptanteDtos.AdoptanteDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var adoptante = await _adoptanteDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Adoptante", id);
            return AdoptanteMapeador.ADto(adoptante);
        }
    }

}
