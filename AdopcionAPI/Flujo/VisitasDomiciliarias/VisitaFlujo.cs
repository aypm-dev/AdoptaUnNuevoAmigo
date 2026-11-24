using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Solicitudes;
using Abstracciones.VisitasDomiciliarias;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace Flujo.VisitasDomiciliarias
{
    public class VisitaFlujo : IVisitaFlujo
    {

        private readonly IVisitaDA _visitaDA;
        private readonly ISolicitudDA _solicitudDA;
        private readonly IVoluntarioDA _voluntarioDA;
        private readonly TimeProvider _reloj;

        public VisitaFlujo(IVisitaDA visitaDA, ISolicitudDA solicitudDA, IVoluntarioDA voluntarioDA, TimeProvider reloj)
        { _visitaDA = visitaDA; _solicitudDA = solicitudDA; _voluntarioDA = voluntarioDA; _reloj = reloj; }

        public async Task<VisitaDto> CrearAsync(VisitaCrearDTO dto, CancellationToken ct)
        {
            var ahora = _reloj.GetUtcNow().UtcDateTime;

            var s = await _solicitudDA.ObtenerPorIdAsync(dto.SolicitudId, ct)
                ?? throw new NoEncontradoException("Solicitud", dto.SolicitudId);

            if (s.Estado is not (EstadoSolicitud.EnRevision or EstadoSolicitud.VisitaProgramada))
                throw new ConflictoException($"Solo se programan visitas para solicitudes EnRevision o VisitaProgramada (actual: {s.Estado}).");
            if (!await _voluntarioDA.ExisteActivoAsync(dto.VoluntarioId, ct))
                throw new ValidacionNegocioException($"El voluntario {dto.VoluntarioId} no existe o está inactivo.");
            if (dto.Fecha < ahora)
                throw new ValidacionNegocioException("La fecha de la visita no puede estar en el pasado.");
            if (await _visitaDA.ExistePendienteAsync(dto.SolicitudId, ct))
                throw new ConflictoException("La solicitud ya tiene una visita pendiente.");

            var v = await _visitaDA.AgregarAsync(new VisitaDomiciliaria
            {
                SolicitudId = dto.SolicitudId,
                VoluntarioId = dto.VoluntarioId,
                Fecha = dto.Fecha,
                Resultado = ResultadoVisita.Pendiente
            }, ct);
            return await ObtenerAsync(v.IdVisita, ct);
        }

        public async Task RegistrarResultadoAsync(int id, VisitaResultadoDTO dto, CancellationToken ct)
        {
            var v = await _visitaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Visita", id);
            var ahora = _reloj.GetUtcNow().UtcDateTime;

            if (v.Resultado != ResultadoVisita.Pendiente)
                throw new ConflictoException("La visita ya tiene un resultado registrado.");
            if (dto.Resultado == ResultadoVisita.Pendiente)
                throw new ValidacionNegocioException("El resultado debe ser Favorable o Desfavorable.");
            if (ahora < v.Fecha)
                throw new ValidacionNegocioException("No se puede registrar el resultado de una visita futura.");
            if (dto.Resultado == ResultadoVisita.Desfavorable && string.IsNullOrWhiteSpace(dto.Observaciones))
                throw new ValidacionNegocioException("Las observaciones son obligatorias cuando el resultado es desfavorable.");

            if (!await _visitaDA.RegistrarResultadoAsync(id, dto.Resultado, dto.Observaciones?.Trim(), ct))
                throw new NoEncontradoException("Visita", id);
        }

        public async Task<ResultadoPaginado<VisitaDto>> ListarAsync(VisitaFiltro f, CancellationToken ct)
        {
            var p = await _visitaDA.BuscarAsync(f, ct);
            return new ResultadoPaginado<VisitaDto>
            { Items = p.Items.Select(VisitaMapeador.ADto).ToList(), Total = p.Total, Pagina = p.Pagina, Tamano = p.Tamano };
        }

        public async Task<VisitaDto> ObtenerAsync(int id, CancellationToken ct) =>
            VisitaMapeador.ADto(await _visitaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Visita", id));
    }

}

