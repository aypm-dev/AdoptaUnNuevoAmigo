using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.TiposVacunas;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.TiposVacunas
{
    public class TipoVacunaDA : ITipoVacunaDA
    {

        private readonly AdopcionContexto _tipoVacunaContexto;
        
        public TipoVacunaDA(AdopcionContexto tipoVacunaContexto)
        {
            _tipoVacunaContexto = tipoVacunaContexto;
        }


        public async Task<bool> ActualizarAsync(TipoVacuna datos, CancellationToken ct)
        {
            var existente = await _tipoVacunaContexto.TiposVacunas.FirstOrDefaultAsync(e => e.IdTipoVacuna == datos.IdTipoVacuna, ct);
            if (existente is null) return false;

            existente.Nombre = datos.Nombre;
            existente.Obligatoria = datos.Obligatoria;
            existente.Activo = datos.Activo;

            try
            {
                await _tipoVacunaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro tipo de vacuna con el nombre '{datos.Nombre}'.");
            }
            return true;
        }

        public async Task<TipoVacuna> AgregarAsync(TipoVacuna tipoVacuna, CancellationToken ct)
        {
            _tipoVacunaContexto.TiposVacunas.Add(tipoVacuna);
            try
            {
                await _tipoVacunaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro tipo de vacuna con el nombre '{tipoVacuna.Nombre}'.");
            }
            return tipoVacuna;
        }

        public async Task<ResultadoPaginado<TipoVacuna>> BuscarAsync(TipoVacunaDtos.TipoVacunaFiltro filtro, CancellationToken ct)
        {
            IQueryable<TipoVacuna> consulta = _tipoVacunaContexto.TiposVacunas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            // Orden estable (Nombre, luego Id) para que la paginación no repita ni se salte filas.
            var items = await consulta
                .OrderBy(c => c.Nombre).ThenBy(e => e.IdTipoVacuna)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<TipoVacuna>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _tipoVacunaContexto.TiposVacunas.FirstOrDefaultAsync(t => t.IdTipoVacuna == id, ct);
            if (existente is null) return false;

            // 1. Cambio a eliminación lógica
            existente.Activo = false;
            _tipoVacunaContexto.TiposVacunas.Update(existente);

            try
            {
                await _tipoVacunaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("Error.");
            }
            return true;
        }

        public Task<TipoVacuna?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _tipoVacunaContexto.TiposVacunas.AsNoTracking().FirstOrDefaultAsync(t => t.IdTipoVacuna == id, ct);
    }
}
