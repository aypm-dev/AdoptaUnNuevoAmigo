using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Modelos;
using Abstracciones.TiposViviendas;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.TiposViviendas
{
    public class TipoViviendaDA : ITipoViviendaDA
    {

        private readonly AdopcionContexto _contexto;

        public TipoViviendaDA(AdopcionContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> ActualizarAsync(TipoVivienda datos, CancellationToken ct)
        {
            var existente = await _contexto.TiposViviendas.FirstOrDefaultAsync(t => t.IdTipoVivienda == datos.IdTipoVivienda, ct);
            if (existente is null) return false;

            existente.Nombre = datos.Nombre;
            existente.Descripcion = datos.Descripcion;
            existente.Activo = datos.Activo;

            try
            {
                await _contexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro tipo de vivienda con el nombre '{datos.Nombre}'.");
            }
            return true;
        }

        public async Task<TipoVivienda> AgregarAsync(TipoVivienda tipoVivienda, CancellationToken ct)
        {
            _contexto.TiposViviendas.Add(tipoVivienda);
            try
            {
                await _contexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe un tipo de vivienda con el nombre '{tipoVivienda.Nombre}'.");
            }
            return tipoVivienda;
        }

        public async Task<ResultadoPaginado<TipoVivienda>> BuscarAsync(TipoViviendaDtos.TipoViviendaFiltro filtro, CancellationToken ct)
        {
            IQueryable<TipoVivienda> consulta = _contexto.TiposViviendas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(texto) ||
                    c.Descripcion.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            // Orden estable (Nombre, luego Id) para que la paginación no repita ni se salte filas.
            var items = await consulta
                .OrderBy(c => c.Nombre).ThenBy(e => e.IdTipoVivienda)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<TipoVivienda>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _contexto.TiposViviendas.FirstOrDefaultAsync(t => t.IdTipoVivienda == id, ct);
            if (existente is null) return false;

            // 1. Cambio a eliminación lógica
            existente.Activo = false;
            _contexto.TiposViviendas.Update(existente);

            try
            {
                await _contexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("Error.");
            }
            return true;
        }

        public Task<TipoVivienda?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _contexto.TiposViviendas.AsNoTracking().FirstOrDefaultAsync(c => c.IdTipoVivienda == id, ct);
    }

    
}
