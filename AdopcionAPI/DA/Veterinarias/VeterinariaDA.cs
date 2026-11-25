using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Veterinarias;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Veterinarias
{
    public class VeterinariaDA : IVeterinariaDA
    {

        private readonly AdopcionContexto _veterinariaContexto;
        public VeterinariaDA(AdopcionContexto veterinariaContexto)
        {
            _veterinariaContexto = veterinariaContexto;
        }

        public async Task<bool> ActualizarAsync(Veterinaria datos, CancellationToken ct)
        {
            var existente = await _veterinariaContexto.Veterinarias.FirstOrDefaultAsync(v => v.IdVeterinaria == datos.IdVeterinaria, ct);
            if (existente is null) return false;
            existente.Nombre = datos.Nombre;
            existente.FechaRegistro = datos.FechaRegistro;
            existente.Diagnostico = datos.Diagnostico;
            existente.Tratamiento = datos.Tratamiento;
            existente.Activo = datos.Activo;

            try
            {
                await _veterinariaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otra veterinaria con el nombre '{datos.FechaRegistro}'.");
            }
            return true;
        }

        public async Task<Veterinaria> AgregarAsync(Veterinaria veterinaria, CancellationToken ct)
        {
            _veterinariaContexto.Veterinarias.Add(veterinaria);
            try
            {
                await _veterinariaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe una veterinaria con el nombre '{veterinaria.FechaRegistro}'.");
            }
            return veterinaria;
        }

        public async Task<ResultadoPaginado<Veterinaria>> BuscarAsync(VeterinariaDtos.VeterinariaFiltro filtro, CancellationToken ct)
        {
            IQueryable<Veterinaria> consulta = _veterinariaContexto.Veterinarias.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(v =>
                    v.Tratamiento.Contains(texto) ||
                    v.Tratamiento.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            // Orden estable (Nombre, luego Id) para que la paginación no repita ni se salte filas.
            var items = await consulta
                .OrderBy(v => v.Diagnostico).ThenBy(v => v.IdVeterinaria)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Veterinaria>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _veterinariaContexto.Veterinarias.FirstOrDefaultAsync(v => v.IdVeterinaria == id, ct);
            if (existente is null) return false;

            // 1. Cambio a eliminación lógica
            existente.Activo = false;
            _veterinariaContexto.Veterinarias.Update(existente);

            try
            {
                await _veterinariaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("Error.");
            }
            return true;
        }

        public Task<Veterinaria?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _veterinariaContexto.Veterinarias.AsNoTracking().FirstOrDefaultAsync(v => v.IdVeterinaria == id, ct);
    }
}
