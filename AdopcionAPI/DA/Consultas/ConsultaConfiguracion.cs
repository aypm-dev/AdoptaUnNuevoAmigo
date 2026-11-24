using Abstracciones.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Consultas
{
    public class ConsultaConfiguracion : IEntityTypeConfiguration<ConsultaVeterinaria>
    {
        public void Configure(EntityTypeBuilder<ConsultaVeterinaria> b)
        {
            b.ToTable("ConsultasVeterinarias");
            b.HasKey(e => e.IdConsulta);
            b.Property(e => e.Fecha).IsRequired();
            b.Property(e => e.Diagnostico).IsRequired().HasMaxLength(1000);
            b.Property(e => e.Tratamiento).IsRequired().HasMaxLength(1000);
            b.HasOne(c => c.Veterinaria).WithMany().HasForeignKey(c => c.VeterinariaId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.Animal).WithMany().HasForeignKey(c => c.AnimalId).OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(c => new { c.AnimalId, c.Fecha });
        }

    }
}
