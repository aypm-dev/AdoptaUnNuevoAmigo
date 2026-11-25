using Abstracciones.Veterinarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Veterinarias
{
    public class VeterinariaConfiguracion : IEntityTypeConfiguration<Veterinaria>
    {

        public void Configure(EntityTypeBuilder<Veterinaria> b)
        {
            b.ToTable("Veterinarias");
            b.HasKey(v => v.IdVeterinaria);

            // Sin esto EF crea nvarchar(max) para todo: lento, sin índice posible
            // y sin protección. Los límites de aquí deben coincidir con las anotaciones de los DTOs.
            b.Property(v => v.Nombre).IsRequired().HasMaxLength(100);
            b.Property(v => v.FechaRegistro).IsRequired();
            b.Property(v => v.Diagnostico).IsRequired().HasMaxLength(500);
            b.Property(v => v.Tratamiento).IsRequired().HasMaxLength(500);
            b.Property(v => v.Activo).IsRequired();
        }

    }
}
