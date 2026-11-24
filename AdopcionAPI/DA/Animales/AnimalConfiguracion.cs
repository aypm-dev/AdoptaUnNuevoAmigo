using Abstracciones.Animales;
using Abstracciones.Razas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Animales
{
    public class AnimalConfiguracion : IEntityTypeConfiguration<Animal>
    {

        public void Configure(EntityTypeBuilder<Animal> b)
        {
     
                b.ToTable("Animales");
                b.HasKey(e => e.IdAnimal);
                b.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                b.Property(e => e.Sexo)
                    .IsRequired()
                    .HasMaxLength(10);
                b.Property(e => e.Tamanio)
                    .IsRequired()
                    .HasMaxLength(20);
                b.Property(e => e.Esterilizado)
                    .IsRequired()
                    .HasMaxLength(10);
                b.Property(e => e.Activo)
                    .IsRequired();
                b.HasOne(d => d.Raza)
                    .WithMany(p => p.Animales)
                    .HasForeignKey(d => d.RazaId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.Property(e => e.Estado).IsRequired().HasDefaultValue(EstadoAnimal.Disponible);
        }

    }
}
