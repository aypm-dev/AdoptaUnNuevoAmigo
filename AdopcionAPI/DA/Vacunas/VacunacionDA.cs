using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Vacunas;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace DA.Vacunas
{
    public class VacunacionDA : IVacunacionDA
    {
        private readonly AdopcionContexto _ctx;
        public VacunacionDA(AdopcionContexto ctx) => _ctx = ctx;

        public async Task<ResultadoPaginado<Vacunacion>> BuscarAsync(VacunacionFiltro f, CancellationToken ct)
        {
            var q = _ctx.Vacunaciones.AsNoTracking().AsQueryable();
            if (f.AnimalId.HasValue) q = q.Where(v => v.AnimalId == f.AnimalId);
            if (f.TipoVacunaId.HasValue) q = q.Where(v => v.TipoVacunaId == f.TipoVacunaId);

            var total = await q.CountAsync(ct);
            q = f.Orden == "fecha"
                ? q.OrderBy(v => v.Fecha).ThenBy(v => v.IdVacunacion)
                : q.OrderByDescending(v => v.Fecha).ThenByDescending(v => v.IdVacunacion);

            var items = await q.Include(v => v.Animal).Include(v => v.TipoVacuna)
                .Skip((f.Pagina - 1) * f.Tamano).Take(f.Tamano).ToListAsync(ct);

            return new ResultadoPaginado<Vacunacion> { Items = items, Total = total, Pagina = f.Pagina, Tamano = f.Tamano };
        }

        public Task<Vacunacion?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Vacunaciones.AsNoTracking().Include(v => v.Animal).Include(v => v.TipoVacuna)
                .FirstOrDefaultAsync(v => v.IdVacunacion == id, ct);

        public async Task<Vacunacion> AgregarAsync(Vacunacion v, CancellationToken ct)
        {
            _ctx.Vacunaciones.Add(v);
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            { throw new ConflictoException("Ya existe esa vacunación para ese animal en esa fecha."); }
            return v;
        }

        public async Task<bool> ActualizarAsync(Vacunacion datos, CancellationToken ct)
        {
            var e = await _ctx.Vacunaciones.FirstOrDefaultAsync(v => v.IdVacunacion == datos.IdVacunacion, ct);
            if (e is null) return false;
            e.TipoVacunaId = datos.TipoVacunaId;
            e.Fecha = datos.Fecha;
            e.ProximaDosis = datos.ProximaDosis;
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            { throw new ConflictoException("Ya existe esa vacunación para ese animal en esa fecha."); }
            return true;
        }

        public Task<bool> TipoVacunaExisteAsync(int id, CancellationToken ct) =>
            _ctx.TiposVacunas.AnyAsync(t => t.IdTipoVacuna == id && t.Activo, ct);

        public async Task<bool> TieneVacunasObligatoriasAlDiaAsync(int animalId, DateTime fecha, CancellationToken ct)
        {
            var hayPendiente = await _ctx.TiposVacunas.AsNoTracking()
                .Where(t => t.Obligatoria && t.Activo)
                .AnyAsync(t => !_ctx.Vacunaciones.Any(v =>
                    v.AnimalId == animalId &&
                    v.TipoVacunaId == t.IdTipoVacuna &&
                    (v.ProximaDosis == null || v.ProximaDosis >= fecha)), ct);
            return !hayPendiente;
        }
    }

}
