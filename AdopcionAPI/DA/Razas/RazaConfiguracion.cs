using Abstracciones.Modelos;
using Abstracciones.Razas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DA.Razas
{
    public class RazaConfiguracion : IEntityTypeConfiguration<Raza>
    {

        public void Configure(EntityTypeBuilder<Raza> b)
        {
            b.ToTable("Razas");
            b.HasKey(t => t.IdRaza);

            b.Property(t => t.Nombre).IsRequired().HasMaxLength(120);
            b.Property(t => t.Descripcion).IsRequired().HasMaxLength(2000);


            // Relación 1-N Cliente -> Ticket. Restrict: no se puede borrar un cliente que tenga tickets.
            b.HasOne(t => t.Especie)
                .WithMany(c => c.Razas)
                .HasForeignKey(t => t.EspecieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices pensados para los filtros que realmente usa la pantalla.
            b.HasIndex(t => t.FechaRegistro);
            b.HasIndex(t => t.EspecieId);
        }


    }
}
