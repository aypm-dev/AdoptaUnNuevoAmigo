using Abstracciones.Especies;
using Abstracciones.TiposViviendas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.TiposViviendas
{
    public class TipoViviendaConfiguracion : IEntityTypeConfiguration<TipoVivienda>
    {


        public void Configure(EntityTypeBuilder<TipoVivienda> b)
        {
            b.ToTable("TiposViviendas");
            b.HasKey(t => t.IdTipoVivienda);

            // Sin esto EF crea nvarchar(max) para todo: lento, sin índice posible
            // y sin protección. Los límites de aquí deben coincidir con las anotaciones de los DTOs.
            b.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            b.Property(t => t.Descripcion).IsRequired().HasMaxLength(500);
            b.Property(t => t.Activo);
            b.HasIndex(t => t.Nombre).IsUnique();

        }


    }
}
