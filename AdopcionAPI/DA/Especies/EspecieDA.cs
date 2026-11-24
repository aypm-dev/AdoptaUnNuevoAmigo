using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Modelos;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    public class EspecieDA : IEspecieDA
    {

        private readonly AdopcionContexto _adopcionContexto;

        public EspecieDA(AdopcionContexto adopcionContexto)
        {
            _adopcionContexto = adopcionContexto;
        }

        public async Task<bool> ActualizarAsync(Especie datos, CancellationToken ct)
        {
            var existente = await _adopcionContexto.Especies.FirstOrDefaultAsync(e => e.IdEspecie == datos.IdEspecie, ct);
            if (existente is null) return false;

            existente.Nombre = datos.Nombre;
            existente.Descripcion = datos.Descripcion;
            existente.Activo = datos.Activo;

            try
            {
                await _adopcionContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otra espeice con el nombre '{datos.Nombre}'.");
            }
            return true;
        }

        public async Task<Especie> AgregarAsync(Especie especie, CancellationToken ct)
        {
            _adopcionContexto.Especies.Add(especie);
            try
            {
                await _adopcionContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe una especie con el nombre '{especie.Nombre}'.");
            }
            return especie;
        }

        public async Task<ResultadoPaginado<Especie>> BuscarAsync(IEspecieDtos.EspecieFiltro filtro, CancellationToken ct)
        {
            // AsNoTracking: solo leemos, EF no necesita vigilar los objetos.
            IQueryable<Especie> consulta = _adopcionContexto.Especies.AsNoTracking();

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
                .OrderBy(c => c.Nombre).ThenBy(e => e.IdEspecie)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Especie>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _adopcionContexto.Especies.FirstOrDefaultAsync(e => e.IdEspecie == id, ct);
            if (existente is null) return false;

            // 1. Cambio a eliminación lógica
            existente.Activo = false;
            _adopcionContexto.Especies.Update(existente);

            try
            {
                await _adopcionContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("Error.");
            }
            return true;
        }

        public Task<Especie?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _adopcionContexto.Especies.AsNoTracking().FirstOrDefaultAsync(c => c.IdEspecie == id, ct);
    }
}