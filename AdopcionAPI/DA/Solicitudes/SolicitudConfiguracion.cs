using Abstracciones.Solicitudes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Solicitudes
{
    public class SolicitudConfiguracion : IEntityTypeConfiguration<SolicitudAdopcion>
    {
        public void Configure(EntityTypeBuilder<SolicitudAdopcion> b)
        {
            b.ToTable("SolicitudesAdopcion");
            b.HasKey(e => e.IdSolicitud);
            b.Property(e => e.FechaSolicitud).IsRequired();
            b.Property(e => e.Estado).IsRequired();

            b.HasOne(s => s.Animal).WithMany().HasForeignKey(s => s.AnimalId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(s => s.Adoptante).WithMany().HasForeignKey(s => s.AdoptanteId).OnDelete(DeleteBehavior.Restrict);

            // Regla 1 a nivel BD: solo UNA solicitud activa (Enviada=0, EnRevision=1, VisitaProgramada=2) por animal.
            b.HasIndex(s => s.AnimalId)
             .IsUnique()
             .HasFilter("[Estado] IN (0, 1, 2)")
             .HasDatabaseName("UX_Solicitud_AnimalActiva");

            b.HasIndex(s => new { s.AdoptanteId, s.Estado });
        }
    }
}
