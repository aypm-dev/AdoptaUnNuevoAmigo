using Abstracciones.Comunes;
using Abstracciones.Consultas;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Consultas.ConsultaDtos;

namespace DA.Consultas
{
    public class ConsultaDA
    {

        private readonly AdopcionContexto _ctx;
        public ConsultaDA(AdopcionContexto ctx) => _ctx = ctx;

        public async Task<ResultadoPaginado<ConsultaVeterinaria>> BuscarAsync(ConsultaFiltro f, CancellationToken ct)
        {
            var q = _ctx.Consultas.AsNoTracking().AsQueryable();
            if (f.AnimalId.HasValue) q = q.Where(c => c.AnimalId == f.AnimalId);
            if (f.VeterinariaId.HasValue) q = q.Where(c => c.VeterinariaId == f.VeterinariaId);

            var total = await q.CountAsync(ct);
            q = f.Orden == "fecha"
                ? q.OrderBy(c => c.Fecha).ThenBy(c => c.IdConsulta)
                : q.OrderByDescending(c => c.Fecha).ThenByDescending(c => c.IdConsulta);

            var items = await q.Include(c => c.Veterinaria).Include(c => c.Animal)
                .Skip((f.Pagina - 1) * f.Tamano).Take(f.Tamano).ToListAsync(ct);
            return new ResultadoPaginado<ConsultaVeterinaria> { Items = items, Total = total, Pagina = f.Pagina, Tamano = f.Tamano };
        }

        public Task<ConsultaVeterinaria?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Consultas.AsNoTracking().Include(c => c.Veterinaria).Include(c => c.Animal)
                .FirstOrDefaultAsync(c => c.IdConsulta == id, ct);

        public async Task<ConsultaVeterinaria> AgregarAsync(ConsultaVeterinaria c, CancellationToken ct)
        {
            _ctx.Consultas.Add(c);
            await _ctx.SaveChangesAsync(ct);
            return c;
        }

        public async Task<bool> ActualizarAsync(ConsultaVeterinaria datos, CancellationToken ct)
        {
            var e = await _ctx.Consultas.FirstOrDefaultAsync(c => c.IdConsulta == datos.IdConsulta, ct);
            if (e is null) return false;
            e.Fecha = datos.Fecha;
            e.Diagnostico = datos.Diagnostico;
            e.Tratamiento = datos.Tratamiento;
            await _ctx.SaveChangesAsync(ct);
            return true;
        }
    }

}

