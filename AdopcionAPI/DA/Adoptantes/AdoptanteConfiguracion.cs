using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Adoptantes
{
    public class AdoptanteConfiguracion : IEntityTypeConfiguration<Adoptante>
    {

        public void Configure(EntityTypeBuilder<Adoptante> b)
        {

            b.ToTable("Adoptantes");
            b.HasKey(e => e.IdAdoptante);
            b.Property(e => e.Identificacion)
                .IsRequired()
                .HasMaxLength(256);
            b.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            b.Property(e => e.Contacto)
                .IsRequired()
                .HasMaxLength(150);
            b.Property(e => e.OtrasMascotas)
                .IsRequired()
                .HasMaxLength(500);
            b.Property(e => e.Activo)
                .IsRequired();
            b.HasOne(d => d.TipoVivienda)
                .WithMany(p => p.Adoptantes)
                .HasForeignKey(d => d.TipoViviendaId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(e => e.Identificacion)
                .IsUnique();
        }

    }
}
