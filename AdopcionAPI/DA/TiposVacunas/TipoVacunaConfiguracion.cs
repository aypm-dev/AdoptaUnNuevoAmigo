using Abstracciones.Especies;
using Abstracciones.TiposVacunas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.TiposVacunas
{
    public class TipoVacunaConfiguracion : IEntityTypeConfiguration<TipoVacuna>
    {

        public void Configure(EntityTypeBuilder<TipoVacuna> b)
        {
            b.ToTable("TiposVacunas");
            b.HasKey(e => e.IdTipoVacuna);

            // Sin esto EF crea nvarchar(max) para todo: lento, sin índice posible
            // y sin protección. Los límites de aquí deben coincidir con las anotaciones de los DTOs.
            b.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            b.Property(e => e.Obligatoria).IsRequired();
            b.Property(e => e.Activo);

        }

    }
}
