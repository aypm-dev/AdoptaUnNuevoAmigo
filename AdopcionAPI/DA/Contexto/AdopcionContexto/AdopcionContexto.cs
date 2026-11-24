using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Abstracciones.Consultas;
using Abstracciones.Especies;
using Abstracciones.Modelos;
using Abstracciones.Razas;
using Abstracciones.Seguimientos;
using Abstracciones.Solicitudes;
using Abstracciones.TiposVacunas;
using Abstracciones.TiposViviendas;
using Abstracciones.Vacunas;
using Abstracciones.Veterinarias;
using Abstracciones.VisitasDomiciliarias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Contexto.AdopcionContexto
{
    public class AdopcionContexto : DbContext
    {
        //las opciones llegan por inyección de dependencias desde program.cs
        public AdopcionContexto(DbContextOptions<AdopcionContexto> options) : base(options)
        {
        }
        //cada dbset se convierte en una tabla, el nombre de la propiedad define el nombre de la tabla
        public DbSet<Especie> Especies => Set<Especie>();

        public DbSet<TipoVivienda> TiposViviendas => Set<TipoVivienda>();

        public DbSet<Veterinaria> Veterinarias => Set<Veterinaria>();

        public DbSet<Raza> Razas => Set<Raza>();

        public DbSet<Animal> Animales => Set<Animal>();

        public DbSet<Adoptante> Adoptantes => Set<Adoptante>();

        public DbSet<TipoVacuna> TiposVacunas => Set<TipoVacuna>(); 

        public DbSet<Vacunacion> Vacunaciones => Set<Vacunacion>();

        public DbSet<SolicitudAdopcion> Solicitudes => Set<SolicitudAdopcion>();

        public DbSet<Voluntario> Voluntarios => Set<Voluntario>();

        public DbSet<VisitaDomiciliaria> Visitas => Set<VisitaDomiciliaria>();

        public DbSet<SeguimientoPostAdopcion> Seguimientos => Set<SeguimientoPostAdopcion>();

        public DbSet<AlertaSeguimiento> Alertas => Set<AlertaSeguimiento>();

        public DbSet<EventoAdopcion> Eventos => Set<EventoAdopcion>();

        public DbSet<ConsultaVeterinaria> Consultas => Set<ConsultaVeterinaria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ****PROFESOR: **** Una clase de configuración por entidad, junto a su módulo (carpeta).
            // Esta línea las descubre todas: agregar un módulo no obliga a tocar este archivo.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdopcionContexto).Assembly);
        }
    }
}
