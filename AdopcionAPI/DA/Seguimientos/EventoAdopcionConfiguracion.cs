using Abstracciones.Animales;
using Abstracciones.Seguimientos;
using Abstracciones.Solicitudes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Seguimientos
{
    public class EventoAdopcionConfiguracion : IEntityTypeConfiguration<EventoAdopcion>
    {

        public void Configure(EntityTypeBuilder<EventoAdopcion> b)
        {
            b.ToTable("EventosAdopcion");
            b.HasKey(e => e.IdEvento);
            b.Property(e => e.Detalle).IsRequired().HasMaxLength(1100);
            b.HasOne<SolicitudAdopcion>().WithMany().HasForeignKey(e => e.SolicitudId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Animal>().WithMany().HasForeignKey(e => e.AnimalId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
