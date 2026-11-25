using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Seguimientos;
using Abstracciones.Solicitudes;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Solicitudes.SolicitudAdopcionDtos;

namespace DA.Solicitudes
{
    public class SolicitudDA : ISolicitudDA
    {
        private static readonly EstadoSolicitud[] Activas =
           { EstadoSolicitud.Enviada, EstadoSolicitud.EnRevision, EstadoSolicitud.VisitaProgramada };

        private readonly AdopcionContexto _ctx;
        public SolicitudDA(AdopcionContexto ctx) => _ctx = ctx;

        public async Task<SolicitudAdopcion> AgregarAsync(SolicitudAdopcion solicitud, CancellationToken ct)
        {
            var animal = await _ctx.Animales.FirstAsync(a => a.IdAnimal == solicitud.AnimalId, ct);
            animal.Estado = EstadoAnimal.EnProceso;
            _ctx.Solicitudes.Add(solicitud);

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException("El animal ya tiene una solicitud activa.");
            }
            return solicitud;
        }

        public async Task<bool> CambiarEstadoAsync(int id, EstadoSolicitud nuevo, EstadoAnimal estadoAnimal,
            DateTime? fechaResolucion, IReadOnlyList<SeguimientoPostAdopcion>? seguimientos, CancellationToken ct)
        {
            var s = await _ctx.Solicitudes.Include(x => x.Animal).FirstOrDefaultAsync(x => x.IdSolicitud == id, ct);
            if (s is null) return false;

            s.Estado = nuevo;
            s.FechaResolucion = fechaResolucion;
            s.Animal.Estado = estadoAnimal;

            if (seguimientos is not null)
            {
                foreach (var seg in seguimientos) seg.SolicitudId = id;
                _ctx.Seguimientos.AddRange(seguimientos);   // misma transacción que la aprobación
            }

            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ResultadoPaginado<SolicitudAdopcion>> BuscarAsync(SolicitudFiltro f, CancellationToken ct)
        {
            var q = _ctx.Solicitudes.AsNoTracking().AsQueryable();

            if (f.AnimalId.HasValue) q = q.Where(s => s.AnimalId == f.AnimalId);
            if (f.AdoptanteId.HasValue) q = q.Where(s => s.AdoptanteId == f.AdoptanteId);
            if (f.Estado.HasValue) q = q.Where(s => s.Estado == f.Estado);

            var total = await q.CountAsync(ct);

            q = f.Orden == "fecha"
                ? q.OrderBy(s => s.FechaSolicitud).ThenBy(s => s.IdSolicitud)
                : q.OrderByDescending(s => s.FechaSolicitud).ThenByDescending(s => s.IdSolicitud);

            var items = await q.Include(s => s.Animal).Include(s => s.Adoptante)
                .Skip((f.Pagina - 1) * f.Tamano).Take(f.Tamano).ToListAsync(ct);

            return new ResultadoPaginado<SolicitudAdopcion> { Items = items, Total = total, Pagina = f.Pagina, Tamano = f.Tamano };
        }

        public Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Solicitudes.AsNoTracking()
                .Include(s => s.Animal).Include(s => s.Adoptante)
                .FirstOrDefaultAsync(s => s.IdSolicitud == id, ct);

        public Task<bool> ExisteActivaParaAnimalAsync(int animalId, CancellationToken ct) =>
            _ctx.Solicitudes.AnyAsync(s => s.AnimalId == animalId && Activas.Contains(s.Estado), ct);

        public Task<DateTime?> UltimoRechazoAsync(int adoptanteId, CancellationToken ct) =>
            _ctx.Solicitudes
                .Where(s => s.AdoptanteId == adoptanteId && s.Estado == EstadoSolicitud.Rechazada)
                .MaxAsync(s => s.FechaResolucion, ct);

        public Task<int> ContarAnimalesEnSeguimientoAsync(int adoptanteId, CancellationToken ct) =>
            _ctx.Solicitudes.CountAsync(s =>
                s.AdoptanteId == adoptanteId &&
                s.Estado == EstadoSolicitud.Aprobada &&
                s.Animal.Estado == EstadoAnimal.EnSeguimiento, ct);
    }
}
