using Abstracciones.Vacunas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Vacunas
{
    public class VacunacionConfiguracion : IEntityTypeConfiguration<Vacunacion>
    {

        public void Configure(EntityTypeBuilder<Vacunacion> b)
        {
            b.ToTable("Vacunaciones");
            b.HasKey(e => e.IdVacunacion);
            b.Property(e => e.Fecha).IsRequired();
            b.HasOne(v => v.Animal).WithMany().HasForeignKey(v => v.AnimalId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(v => v.TipoVacuna).WithMany().HasForeignKey(v => v.TipoVacunaId).OnDelete(DeleteBehavior.Restrict);
            // Evita registrar dos veces la misma vacuna el mismo día.
            b.HasIndex(v => new { v.AnimalId, v.TipoVacunaId, v.Fecha }).IsUnique();
        }

    }
}
