using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Vacunas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace Flujo.VacunacionFlujo
{
    public class VacunacionFlujo : IVacunacionFlujo
    {
        private readonly IVacunacionDA _da;
        private readonly IAnimalDA _animalDA;
        private readonly TimeProvider _reloj;

        public VacunacionFlujo(IVacunacionDA da, IAnimalDA animalDA, TimeProvider reloj)
        { _da = da; _animalDA = animalDA; _reloj = reloj; }

        public async Task<VacunacionDto> CrearAsync(VacunacionCrearDTO dto, CancellationToken ct)
        {
            _ = await _animalDA.ObtenerPorIdAsync(dto.AnimalId, ct) ?? throw new NoEncontradoException("Animal", dto.AnimalId);
            await Validar(dto.TipoVacunaId, dto.Fecha, dto.ProximaDosis, ct);

            var v = await _da.AgregarAsync(new Vacunacion
            {
                AnimalId = dto.AnimalId,
                TipoVacunaId = dto.TipoVacunaId,
                Fecha = dto.Fecha,
                ProximaDosis = dto.ProximaDosis
            }, ct);
            return await ObtenerAsync(v.IdVacunacion, ct);
        }

        public async Task ActualizarAsync(int id, VacunacionActualizarDTO dto, CancellationToken ct)
        {
            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Vacunación", id);
            await Validar(dto.TipoVacunaId, dto.Fecha, dto.ProximaDosis, ct);

            if (!await _da.ActualizarAsync(new Vacunacion
            { IdVacunacion = id, TipoVacunaId = dto.TipoVacunaId, Fecha = dto.Fecha, ProximaDosis = dto.ProximaDosis }, ct))
                throw new NoEncontradoException("Vacunación", id);
        }

        private async Task Validar(int tipoId, DateTime fecha, DateTime? proxima, CancellationToken ct)
        {
            if (!await _da.TipoVacunaExisteAsync(tipoId, ct))
                throw new ValidacionNegocioException($"La vacuna {tipoId} no existe o está inactiva.");
            if (fecha > _reloj.GetUtcNow().UtcDateTime)
                throw new ValidacionNegocioException("La fecha de vacunación no puede estar en el futuro.");
            if (proxima is { } p && p <= fecha)
                throw new ValidacionNegocioException("La próxima dosis debe ser posterior a la fecha de vacunación.");
        }

        public async Task<ResultadoPaginado<VacunacionDto>> ListarAsync(VacunacionFiltro f, CancellationToken ct)
        {
            var p = await _da.BuscarAsync(f, ct);
            return new ResultadoPaginado<VacunacionDto>
            { Items = p.Items.Select(VacunacionMapeador.ADto).ToList(), Total = p.Total, Pagina = p.Pagina, Tamano = p.Tamano };
        }

        public async Task<VacunacionDto> ObtenerAsync(int id, CancellationToken ct) =>
            VacunacionMapeador.ADto(await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Vacunación", id));
    }
}
