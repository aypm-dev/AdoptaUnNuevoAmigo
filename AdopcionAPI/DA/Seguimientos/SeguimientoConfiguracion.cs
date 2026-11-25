using Abstracciones.Seguimientos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Seguimientos
{
    public class SeguimientoConfiguracion : IEntityTypeConfiguration<SeguimientoPostAdopcion>
    {

        public void Configure(EntityTypeBuilder<SeguimientoPostAdopcion> b)
        {
            b.ToTable("SeguimientosPostAdopcion");
            b.HasKey(e => e.IdSeguimiento);
            b.Property(e => e.Hito).HasConversion<string>().HasMaxLength(20).IsRequired();
            b.Property(e => e.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
            b.Property(e => e.Observaciones).HasMaxLength(1000);
            b.HasOne(s => s.Solicitud).WithMany().HasForeignKey(s => s.SolicitudId).OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(s => new { s.SolicitudId, s.Hito }).IsUnique();   // un 30, un 60 y un 90 por solicitud
            b.HasIndex(s => new { s.Estado, s.FechaProgramada });        // lo usa el job
        }

    }
}
