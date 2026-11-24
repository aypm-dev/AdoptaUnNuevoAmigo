using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static Abstracciones.Razas.RazaDtos;

namespace DA.Razas
{
    internal static class RazaConsultas
    {

        public static IQueryable<Raza> AplicarFiltros(IQueryable<Raza> q, RazaFiltro f)
        {
            if (f.EspecieId.HasValue)
            {
                var EspecieId = f.EspecieId.Value;
                q = q.Where(t => t.EspecieId == EspecieId);
            }
            return q;
        }

        public static IQueryable<Raza> Ordenar(IQueryable<Raza> q, string? orden) => orden switch
        {
            "fecha" => q.OrderBy(t => t.FechaRegistro).ThenBy(t => t.IdRaza),
            _ => q.OrderByDescending(t => t.FechaRegistro).ThenByDescending(t => t.IdRaza) // "-fecha" por defecto
        };

    }
}
