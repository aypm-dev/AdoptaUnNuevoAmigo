using Abstracciones.Animales;
using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Animales.AnimalDtos;
using static Abstracciones.Razas.RazaDtos;

namespace DA.Animales
{
    internal static class AnimalConsultas
    {

        public static IQueryable<Animal> AplicarFiltros(IQueryable<Animal> q, AnimalFiltro f)
        {
            if (f.RazaId.HasValue)
            {
                var RazaId = f.RazaId.Value;
                q = q.Where(t => t.RazaId == RazaId);
            }
            return q;
        }

        public static IQueryable<Animal> Ordenar(IQueryable<Animal> q, string? orden) => orden switch
        {
            "fecha" => q.OrderBy(t => t.FechaNacimiento).ThenBy(t => t.IdAnimal),
            _ => q.OrderByDescending(t => t.FechaNacimiento).ThenByDescending(t => t.IdAnimal) // "-fecha" por defecto
        };

    }
}
