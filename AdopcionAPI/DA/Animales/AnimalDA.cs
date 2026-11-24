using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Razas;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using DA.Razas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Animales
{
    public class AnimalDA : IAnimalDA
    {

        private readonly AdopcionContexto _animalContexto;

        public AnimalDA(AdopcionContexto animalContexto)
        {
            _animalContexto = animalContexto;
        }   

        public async Task<bool> ActualizarAsync(Animal datos, CancellationToken ct)
        {
            var existente = await _animalContexto.Animales.FirstOrDefaultAsync(a => a.IdAnimal == datos.IdAnimal, ct);
            if (existente is null) return false;

            existente.Nombre = datos.Nombre;
            existente.Sexo = datos.Sexo;
            existente.Esterilizado = datos.Esterilizado;
            existente.Tamanio = datos.Tamanio;
            existente.RazaId = datos.RazaId;
            existente.FechaNacimiento = datos.FechaNacimiento;
            existente.Activo = datos.Activo;

            try
            {
                await _animalContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro animal con el nombre '{datos.Nombre}'.");
            }
            return true;
        }

        public async Task<Animal> AgregarAsync(Animal animal, CancellationToken ct)
        {
            _animalContexto.Animales.Add(animal);
            try
            {
                await _animalContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe un animal con el nombre '{animal.Nombre}'.");
            }
            return animal;
        }

        public async Task<ResultadoPaginado<Animal>> BuscarAsync(AnimalDtos.AnimalFiltro filtro, CancellationToken ct)
        {
            var consulta = _animalContexto.Animales.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                // Contains se traduce a LIKE con los comodines escapados. En SQL Server la comparación
                // no distingue mayúsculas por la collation por defecto.
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(t => t.Nombre.Contains(texto) || t.Sexo.Contains(texto) || t.Tamanio.Contains(texto) || t.Esterilizado.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            var items = await AnimalConsultas.Ordenar(consulta, filtro.Orden)
                .Include(t => t.Raza)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Animal>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _animalContexto.Animales.FirstOrDefaultAsync(a => a.IdAnimal == id, ct);
            if (existente is null) return false;

            // 1. Cambio a eliminación lógica
            existente.Activo = false;
            _animalContexto.Animales.Update(existente);

            try
            {
                await _animalContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("Error.");
            }
            return true;
        }

        public Task<Animal?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _animalContexto.Animales.AsNoTracking().FirstOrDefaultAsync(a => a.IdAnimal == id, ct);
    }
}
