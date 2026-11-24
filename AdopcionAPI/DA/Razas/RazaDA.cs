using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Razas;
using DA.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DA.Razas
{
    public class RazaDA : IRazaDA
    {

        private readonly AdopcionContexto _razaContexto;

        public RazaDA(AdopcionContexto razaContexto)
        {
            _razaContexto = razaContexto;
        }
        

        public async Task<Raza> AgregarAsync(Raza raza, CancellationToken ct)
        {
            _razaContexto.Razas.Add(raza);
            try
            {
                await _razaContexto.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                // Dos tickets creados al mismo instante calcularon la misma secuencia.
                throw new ConflictoException("No se pudo generar un código único para el ticket. Intente de nuevo.");
            }
            return raza;
        }

        public async Task<ResultadoPaginado<Raza>> BuscarAsync(RazaDtos.RazaFiltro filtro, CancellationToken ct)
        {
            var consulta = RazaConsultas.AplicarFiltros(_razaContexto.Razas.AsNoTracking(), filtro);

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                // Contains se traduce a LIKE con los comodines escapados. En SQL Server la comparación
                // no distingue mayúsculas por la collation por defecto.
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(t => t.Nombre.Contains(texto) || t.Descripcion.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            var items = await RazaConsultas.Ordenar(consulta, filtro.Orden)
                .Include(t => t.Especie)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Raza>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public Task<Raza?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
        _razaContexto.Razas.AsNoTracking().FirstOrDefaultAsync(c => c.IdRaza == id, ct);
    }
}
