using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Seguimientos;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace DA.Seguimientos
{
    public class SeguimientoDA : ISeguimientoDA
    {

        private readonly AdopcionContexto _ctx;
        public SeguimientoDA(AdopcionContexto ctx) => _ctx = ctx;

        private IQueryable<SeguimientoPostAdopcion> ConDetalle(IQueryable<SeguimientoPostAdopcion> q) => q
            .Include(s => s.Solicitud).ThenInclude(x => x.Animal)
            .Include(s => s.Solicitud).ThenInclude(x => x.Adoptante);

        public async Task<ResultadoPaginado<SeguimientoPostAdopcion>> BuscarAsync(SeguimientoFiltro f, CancellationToken ct)
        {
            var q = _ctx.Seguimientos.AsNoTracking().AsQueryable();
            if (f.SolicitudId.HasValue) q = q.Where(s => s.SolicitudId == f.SolicitudId);
            if (f.Estado.HasValue) q = q.Where(s => s.Estado == f.Estado);
            if (f.Hito.HasValue) q = q.Where(s => s.Hito == f.Hito);

            var total = await q.CountAsync(ct);
            q = f.Orden == "-fecha"
                ? q.OrderByDescending(s => s.FechaProgramada).ThenByDescending(s => s.IdSeguimiento)
                : q.OrderBy(s => s.FechaProgramada).ThenBy(s => s.IdSeguimiento);

            var items = await ConDetalle(q).Skip((f.Pagina - 1) * f.Tamano).Take(f.Tamano).ToListAsync(ct);
            return new ResultadoPaginado<SeguimientoPostAdopcion> { Items = items, Total = total, Pagina = f.Pagina, Tamano = f.Tamano };
        }

        public Task<SeguimientoPostAdopcion?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            ConDetalle(_ctx.Seguimientos.AsNoTracking()).FirstOrDefaultAsync(s => s.IdSeguimiento == id, ct);

        public async Task<bool> RegistrarResultadoAsync(int id, EstadoSeguimiento estado, string? observaciones, DateTime fecha,
            EstadoAnimal? nuevoEstadoAnimal, EventoAdopcion? evento, CancellationToken ct)
        {
            var s = await _ctx.Seguimientos.Include(x => x.Solicitud).ThenInclude(x => x.Animal)
                .FirstOrDefaultAsync(x => x.IdSeguimiento == id, ct);
            if (s is null) return false;

            s.Estado = estado;
            s.Observaciones = observaciones;
            s.FechaRealizada = fecha;

            if (nuevoEstadoAnimal is { } estadoAnimal)
            {
                s.Solicitud.Animal.Estado = estadoAnimal;

                // La adopción se cierra: los hitos aún abiertos ya no aplican.
                var abiertos = await _ctx.Seguimientos
                    .Where(x => x.SolicitudId == s.SolicitudId && x.IdSeguimiento != id &&
                                (x.Estado == EstadoSeguimiento.Pendiente || x.Estado == EstadoSeguimiento.Vencido))
                    .ToListAsync(ct);
                foreach (var o in abiertos) o.Estado = EstadoSeguimiento.Omitido;
            }

            if (evento is not null) _ctx.Eventos.Add(evento);

            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<int> MarcarVencidosYCrearAlertasAsync(DateTime ahora, CancellationToken ct)
        {
            var vencidos = await ConDetalle(_ctx.Seguimientos)
                .Where(s => s.Estado == EstadoSeguimiento.Pendiente && s.FechaProgramada < ahora)
                .ToListAsync(ct);

            foreach (var s in vencidos)
            {
                s.Estado = EstadoSeguimiento.Vencido;
                _ctx.Alertas.Add(new AlertaSeguimiento
                {
                    SeguimientoId = s.IdSeguimiento,
                    FechaCreacion = ahora,
                    Mensaje = $"Seguimiento de {(int)s.Hito} días vencido sin realizar: {s.Solicitud.Animal.Nombre} " +
                              $"(adoptante {s.Solicitud.Adoptante.Nombre}, solicitud {s.SolicitudId}). " +
                              $"Programado para {s.FechaProgramada:yyyy-MM-dd}."
                });
            }

            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {   // otra instancia ya generó esas alertas
                _ctx.ChangeTracker.Clear();
                return 0;
            }
            return vencidos.Count;
        }

        public async Task<IReadOnlyList<AlertaSeguimiento>> ObtenerAlertasSinEscalarAsync(CancellationToken ct) =>
            await _ctx.Alertas.AsNoTracking().Where(a => !a.EscaladaAlCoordinador).OrderBy(a => a.IdAlerta).ToListAsync(ct);

        public async Task MarcarEscaladaAsync(int idAlerta, DateTime fecha, CancellationToken ct)
        {
            var a = await _ctx.Alertas.FirstOrDefaultAsync(x => x.IdAlerta == idAlerta, ct);
            if (a is null) return;
            a.EscaladaAlCoordinador = true;
            a.FechaEscalada = fecha;
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<AlertaSeguimiento>> ListarAlertasAsync(CancellationToken ct) =>
            await _ctx.Alertas.AsNoTracking().OrderByDescending(a => a.FechaCreacion).Take(200).ToListAsync(ct);

    }
}
