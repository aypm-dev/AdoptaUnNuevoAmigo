using Abstracciones.Seguimientos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Seguimientos
{
    public class AlertaConfiguracion : IEntityTypeConfiguration<AlertaSeguimiento>
    {

        public void Configure(EntityTypeBuilder<AlertaSeguimiento> b)
        {
            b.ToTable("AlertasSeguimiento");
            b.HasKey(e => e.IdAlerta);
            b.Property(e => e.Mensaje).IsRequired().HasMaxLength(500);
            b.HasOne<SeguimientoPostAdopcion>().WithMany().HasForeignKey(a => a.SeguimientoId).OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(a => a.SeguimientoId).IsUnique();                 // jamás dos alertas por seguimiento
        }

    }
}
