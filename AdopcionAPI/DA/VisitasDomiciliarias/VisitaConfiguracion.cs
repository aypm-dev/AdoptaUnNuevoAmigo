using Abstracciones.VisitasDomiciliarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.VisitasDomiciliarias
{
    public class VisitaConfiguracion
    {

        public void Configure(EntityTypeBuilder<VisitaDomiciliaria> b)
        {
            b.ToTable("VisitasDomiciliarias");
            b.HasKey(e => e.IdVisita);
            b.Property(e => e.Fecha).IsRequired();
            b.Property(e => e.Resultado).IsRequired();
            b.Property(e => e.Observaciones).HasMaxLength(1000);
            b.HasOne(v => v.Solicitud).WithMany().HasForeignKey(v => v.SolicitudId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(v => v.Voluntario).WithMany().HasForeignKey(v => v.VoluntarioId).OnDelete(DeleteBehavior.Restrict);

            // Una sola visita pendiente por solicitud, garantizado en BD.
            b.HasIndex(v => v.SolicitudId).IsUnique().HasFilter("[Resultado] = 0")
             .HasDatabaseName("UX_Visita_PendientePorSolicitud");
        }

    }
}
