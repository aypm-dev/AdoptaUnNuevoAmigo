using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static Abstracciones.Razas.RazaDtos;

namespace Flujo.Razas
{
    public class RazaFlujo : IRazaFlujo
    {

        private readonly IRazaDA _razaDA;
        private readonly TimeProvider _reloj;
        private readonly IEspecieDA _especieDA;

        public RazaFlujo(IRazaDA razaDA, IEspecieDA especieDA, TimeProvider reloj)
        {
            _razaDA = razaDA;
            _reloj = reloj;
            _especieDA = especieDA;
        }

        public async Task<RazaDtos.RazaDto> CrearAsync(RazaDtos.RazaCrearDTO dto, CancellationToken ct)
        {
            if (await _especieDA.ObtenerPorIdAsync(dto.EspecieId, ct) is null)
                throw new ValidacionNegocioException($"La especie {dto.EspecieId} no existe.");

            var ahora = _reloj.GetUtcNow().UtcDateTime;

            var raza = new Raza
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                EspecieId = dto.EspecieId,
                FechaRegistro = ahora
            };

            await _razaDA.AgregarAsync(raza, ct);
            return await ObtenerAsync(raza.IdRaza, ct);
        }

        public async Task<ResultadoPaginado<RazaDtos.RazaDto>> ListarAsync(RazaDtos.RazaFiltro filtro, CancellationToken ct)
        {
            var pagina = await _razaDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<RazaDtos.RazaDto>
            {
                Items = pagina.Items.Select(RazaMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<RazaDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var raza = await _razaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Raza", id);
            return RazaMapeador.ADto(raza);
        }

    }
}
