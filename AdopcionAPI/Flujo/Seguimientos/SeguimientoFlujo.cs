using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Seguimientos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace Flujo.Seguimientos
{
    public class SeguimientoFlujo : ISeguimientoFlujo
    {

        private readonly ISeguimientoDA _da;
        private readonly TimeProvider _reloj;
        public SeguimientoFlujo(ISeguimientoDA da, TimeProvider reloj) { _da = da; _reloj = reloj; }

        public async Task RegistrarResultadoAsync(int id, SeguimientoResultadoDTO dto, CancellationToken ct)
        {
            var s = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Seguimiento", id);
            var ahora = _reloj.GetUtcNow().UtcDateTime;

            if (s.Estado is not (EstadoSeguimiento.Pendiente or EstadoSeguimiento.Vencido))
                throw new ConflictoException($"El seguimiento ya está en estado {s.Estado}.");
            if (s.Solicitud.Animal.Estado != EstadoAnimal.EnSeguimiento)
                throw new ConflictoException("El animal ya no está en seguimiento.");
            if (ahora < s.FechaProgramada)
                throw new ValidacionNegocioException($"No se puede registrar antes de la fecha programada ({s.FechaProgramada:yyyy-MM-dd}).");
            if (!dto.Favorable && string.IsNullOrWhiteSpace(dto.Observaciones))
                throw new ValidacionNegocioException("Las observaciones son obligatorias cuando el resultado es desfavorable.");

            // REGLA 7: solo el hito de 90 días cierra la adopción.
            EstadoAnimal? nuevoEstadoAnimal = null;
            EventoAdopcion? evento = null;
            if (s.Hito == HitoSeguimiento.Dias90)
            {
                nuevoEstadoAnimal = dto.Favorable ? EstadoAnimal.Adoptado : EstadoAnimal.Disponible;
                evento = new EventoAdopcion
                {
                    SolicitudId = s.SolicitudId,
                    AnimalId = s.Solicitud.AnimalId,
                    Tipo = dto.Favorable ? TipoEventoAdopcion.AdopcionDefinitiva : TipoEventoAdopcion.AdopcionFallida,
                    Fecha = ahora,
                    Detalle = dto.Favorable
                        ? "Seguimiento de 90 días favorable: adopción definitiva."
                        : $"Seguimiento de 90 días desfavorable: {dto.Observaciones!.Trim()}"
                };
            }

            if (!await _da.RegistrarResultadoAsync(id,
                    dto.Favorable ? EstadoSeguimiento.Favorable : EstadoSeguimiento.Desfavorable,
                    dto.Observaciones?.Trim(), ahora, nuevoEstadoAnimal, evento, ct))
                throw new NoEncontradoException("Seguimiento", id);
        }

        public async Task<ResultadoPaginado<SeguimientoDto>> ListarAsync(SeguimientoFiltro f, CancellationToken ct)
        {
            var p = await _da.BuscarAsync(f, ct);
            return new ResultadoPaginado<SeguimientoDto>
            { Items = p.Items.Select(SeguimientoMapeador.ADto).ToList(), Total = p.Total, Pagina = p.Pagina, Tamano = p.Tamano };
        }

        public async Task<SeguimientoDto> ObtenerAsync(int id, CancellationToken ct) =>
            SeguimientoMapeador.ADto(await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Seguimiento", id));

        public async Task<IReadOnlyList<AlertaDto>> ListarAlertasAsync(CancellationToken ct) =>
            (await _da.ListarAlertasAsync(ct)).Select(SeguimientoMapeador.ADto).ToList();
    }

    // REGLA 6: lógica del job (separada del worker para poder probarla sin timers).
    public class VencimientosSeguimientoFlujo : IVencimientosSeguimientoFlujo
    {
        private readonly ISeguimientoDA _da;
        private readonly INotificadorCoordinador _notificador;
        private readonly TimeProvider _reloj;
        private readonly ILogger<VencimientosSeguimientoFlujo> _log;

        public VencimientosSeguimientoFlujo(ISeguimientoDA da, INotificadorCoordinador notificador,
            TimeProvider reloj, ILogger<VencimientosSeguimientoFlujo> log)
        { _da = da; _notificador = notificador; _reloj = reloj; _log = log; }

        public async Task<int> ProcesarAsync(CancellationToken ct)
        {
            var ahora = _reloj.GetUtcNow().UtcDateTime;

            var nuevos = await _da.MarcarVencidosYCrearAlertasAsync(ahora, ct);

            // También reintenta las que fallaron en ciclos anteriores.
            foreach (var alerta in await _da.ObtenerAlertasSinEscalarAsync(ct))
            {
                try
                {
                    await _notificador.NotificarAsync(alerta, ct);
                    await _da.MarcarEscaladaAsync(alerta.IdAlerta, ahora, ct);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _log.LogError(ex, "No se pudo escalar la alerta {IdAlerta}; se reintentará.", alerta.IdAlerta);
                }
            }
            return nuevos;
        }

    }


    public class NotificadorCoordinadorLog : INotificadorCoordinador
    {
        private readonly ILogger<NotificadorCoordinadorLog> _log;
        public NotificadorCoordinadorLog(ILogger<NotificadorCoordinadorLog> log) => _log = log;

        public Task NotificarAsync(AlertaSeguimiento alerta, CancellationToken ct)
        {
            _log.LogWarning("ESCALADO AL COORDINADOR: {Mensaje}", alerta.Mensaje);
            return Task.CompletedTask;
        }
    }

}
