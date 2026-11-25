using Abstracciones.VisitasDomiciliarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.VisitasDomiciliarias
{
    public class VoluntarioConfiguracion : IEntityTypeConfiguration<Voluntario>
    {

        public void Configure(EntityTypeBuilder<Voluntario> b)
        {
            b.ToTable("Voluntarios");
            b.HasKey(e => e.IdVoluntario);
            b.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            b.Property(e => e.Contacto).IsRequired().HasMaxLength(100);
        }

    }

}