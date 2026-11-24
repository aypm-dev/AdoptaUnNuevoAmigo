using Abstracciones.Especies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Especies
{
    public class EspecieConfiguracion : IEntityTypeConfiguration<Especie>
    {
        public void Configure(EntityTypeBuilder<Especie> b)
        {
            b.ToTable("Especies");
            b.HasKey(e => e.IdEspecie);

            // Sin esto EF crea nvarchar(max) para todo: lento, sin índice posible
            // y sin protección. Los límites de aquí deben coincidir con las anotaciones de los DTOs.
            b.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            b.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);
            b.Property(e => e.Activo);

        }
    }
}
