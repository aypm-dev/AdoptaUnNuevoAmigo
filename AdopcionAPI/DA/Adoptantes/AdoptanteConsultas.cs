using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace DA.Adoptantes
{
    public class AdoptanteConsultas
    {

        public static IQueryable<Adoptante> AplicarFiltros(IQueryable<Adoptante> q, AdoptanteFiltro f)
        {
            if (f.TipoViviendaId.HasValue)
            {
                var TipoViviendaId = f.TipoViviendaId.Value;
                q = q.Where(t => t.TipoViviendaId == TipoViviendaId);
            }
            return q;
        }

        public static IQueryable<Adoptante> Ordenar(IQueryable<Adoptante> q, string? orden) => orden switch
        {
            "fecha" => q.OrderBy(t => t.FechaRegistro).ThenBy(t => t.IdAdoptante),
            _ => q.OrderByDescending(t => t.FechaRegistro).ThenByDescending(t => t.IdAdoptante) // "-fecha" por defecto
        };

    }
}
