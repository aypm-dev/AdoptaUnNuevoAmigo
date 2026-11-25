using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Solicitudes;
using Abstracciones.VisitasDomiciliarias;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace DA.VisitasDomiciliarias
{
    public class VisitaDA: IVisitaDA
    {

        private readonly AdopcionContexto _ctx;
        public VisitaDA(AdopcionContexto ctx) => _ctx = ctx;

        public async Task<VisitaDomiciliaria> AgregarAsync(VisitaDomiciliaria v, CancellationToken ct)
        {
            var solicitud = await _ctx.Solicitudes.FirstAsync(s => s.IdSolicitud == v.SolicitudId, ct);
            if (solicitud.Estado == EstadoSolicitud.EnRevision)
                solicitud.Estado = EstadoSolicitud.VisitaProgramada;

            _ctx.Visitas.Add(v);
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            { throw new ConflictoException("La solicitud ya tiene una visita pendiente."); }
            return v;
        }

        public async Task<bool> RegistrarResultadoAsync(int id, ResultadoVisita resultado, string? observaciones, CancellationToken ct)
        {
            var v = await _ctx.Visitas.FirstOrDefaultAsync(x => x.IdVisita == id, ct);
            if (v is null) return false;
            v.Resultado = resultado;
            v.Observaciones = observaciones;
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ResultadoPaginado<VisitaDomiciliaria>> BuscarAsync(VisitaFiltro f, CancellationToken ct)
        {
            var q = _ctx.Visitas.AsNoTracking().AsQueryable();
            if (f.SolicitudId.HasValue) q = q.Where(v => v.SolicitudId == f.SolicitudId);
            if (f.VoluntarioId.HasValue) q = q.Where(v => v.VoluntarioId == f.VoluntarioId);
            if (f.Resultado.HasValue) q = q.Where(v => v.Resultado == f.Resultado);

            var total = await q.CountAsync(ct);
            q = f.Orden == "fecha"
                ? q.OrderBy(v => v.Fecha).ThenBy(v => v.IdVisita)
                : q.OrderByDescending(v => v.Fecha).ThenByDescending(v => v.IdVisita);

            var items = await q.Include(v => v.Voluntario)
                .Skip((f.Pagina - 1) * f.Tamano).Take(f.Tamano).ToListAsync(ct);
            return new ResultadoPaginado<VisitaDomiciliaria> { Items = items, Total = total, Pagina = f.Pagina, Tamano = f.Tamano };
        }

        public Task<VisitaDomiciliaria?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Visitas.AsNoTracking().Include(v => v.Voluntario).FirstOrDefaultAsync(v => v.IdVisita == id, ct);

        public Task<bool> ExistePendienteAsync(int solicitudId, CancellationToken ct) =>
            _ctx.Visitas.AnyAsync(v => v.SolicitudId == solicitudId && v.Resultado == ResultadoVisita.Pendiente, ct);

        public Task<ResultadoVisita?> UltimoResultadoAsync(int solicitudId, CancellationToken ct) =>
            _ctx.Visitas.AsNoTracking()
                .Where(v => v.SolicitudId == solicitudId && v.Resultado != ResultadoVisita.Pendiente)
                .OrderByDescending(v => v.Fecha).ThenByDescending(v => v.IdVisita)
                .Select(v => (ResultadoVisita?)v.Resultado)
                .FirstOrDefaultAsync(ct);
    }

}

