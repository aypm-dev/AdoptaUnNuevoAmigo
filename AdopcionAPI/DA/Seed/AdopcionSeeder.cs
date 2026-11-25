using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Abstracciones.Consultas;
using Abstracciones.Especies;
using Abstracciones.Razas;
using Abstracciones.Seguimientos;
using Abstracciones.Solicitudes;
using Abstracciones.TiposVacunas;
using Abstracciones.TiposViviendas;
using Abstracciones.Vacunas;
using Abstracciones.Veterinarias;
using Abstracciones.VisitasDomiciliarias;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Seed;

public static class AdopcionSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AdopcionContexto>();

        await ctx.Database.MigrateAsync(ct);

        // If core test data already exists, backfill FotoUrl/Descripcion if missing, then exit.
        if (await ctx.Animales.AnyAsync(ct))
        {
            await BackfillDescripcionesYFotosAsync(ctx, ct);
            return;
        }

        var ahora = DateTime.UtcNow;

        // 1. Especies (reuse existing ones if already present)
        var perro = await ctx.Especies.FirstOrDefaultAsync(e => e.Nombre == "Perro", ct);
        if (perro is null)
        {
            perro = new Especie { Nombre = "Perro", Descripcion = "Canino doméstico", Activo = true, FechaRegistro = ahora.AddMonths(-6) };
            ctx.Especies.Add(perro);
        }

        var gato = await ctx.Especies.FirstOrDefaultAsync(e => e.Nombre == "Gato", ct);
        if (gato is null)
        {
            gato = new Especie { Nombre = "Gato", Descripcion = "Felino doméstico", Activo = true, FechaRegistro = ahora.AddMonths(-6) };
            ctx.Especies.Add(gato);
        }

        var conejo = await ctx.Especies.FirstOrDefaultAsync(e => e.Nombre == "Conejo", ct);
        if (conejo is null)
        {
            conejo = new Especie { Nombre = "Conejo", Descripcion = "Lagomorfo doméstico", Activo = true, FechaRegistro = ahora.AddMonths(-5) };
            ctx.Especies.Add(conejo);
        }
        await ctx.SaveChangesAsync(ct);

        // 2. Razas
        var mestizo = new Raza { Nombre = "Mestizo", Descripcion = "Perro mestizo de refugio", EspecieId = perro.IdEspecie, FechaRegistro = ahora.AddMonths(-6) };
        var salchicha = new Raza { Nombre = "Dachshund", Descripcion = "Perro salchicha juguetón", EspecieId = perro.IdEspecie, FechaRegistro = ahora.AddMonths(-6) };
        var labrador = new Raza { Nombre = "Labrador", Descripcion = "Perro amigable y activo", EspecieId = perro.IdEspecie, FechaRegistro = ahora.AddMonths(-6) };
        var criolloGato = new Raza { Nombre = "Criollo", Descripcion = "Gato doméstico de pelo corto", EspecieId = gato.IdEspecie, FechaRegistro = ahora.AddMonths(-5) };
        var siames = new Raza { Nombre = "Siamés", Descripcion = "Gato sociable de ojos azules", EspecieId = gato.IdEspecie, FechaRegistro = ahora.AddMonths(-5) };
        ctx.Razas.AddRange(mestizo, salchicha, labrador, criolloGato, siames);

        // 3. Tipos de Vivienda
        var casaPatio = new TipoVivienda { Nombre = "Casa con patio", Descripcion = "Casa independiente con patio cerrado", Activo = true, FechaRegistro = ahora.AddMonths(-6) };
        var casaSinPatio = new TipoVivienda { Nombre = "Casa sin patio", Descripcion = "Casa sin zona verde cercada", Activo = true, FechaRegistro = ahora.AddMonths(-6) };
        var apartamento = new TipoVivienda { Nombre = "Apartamento", Descripcion = "Apartamento en edificio o condominio", Activo = true, FechaRegistro = ahora.AddMonths(-6) };
        ctx.TiposViviendas.AddRange(casaPatio, casaSinPatio, apartamento);

        // 4. Tipos de Vacuna
        var rabia = new TipoVacuna { Nombre = "Rabia", Obligatoria = true, Activo = true };
        var parvovirus = new TipoVacuna { Nombre = "Parvovirus Canino", Obligatoria = true, Activo = true };
        var moquillo = new TipoVacuna { Nombre = "Moquillo", Obligatoria = true, Activo = true };
        var bordetella = new TipoVacuna { Nombre = "Bordetella", Obligatoria = false, Activo = true };
        ctx.TiposVacunas.AddRange(rabia, parvovirus, moquillo, bordetella);

        // 5. Veterinarias
        var vetSanJose = new Veterinaria { Nombre = "Clínica Veterinaria San José", FechaRegistro = ahora.AddMonths(-6), Diagnostico = "Control general", Tratamiento = "Preventivo", Activo = true };
        var vetHeredia = new Veterinaria { Nombre = "Hospital Veterinario Heredia", FechaRegistro = ahora.AddMonths(-5), Diagnostico = "Control general", Tratamiento = "Preventivo", Activo = true };
        ctx.Veterinarias.AddRange(vetSanJose, vetHeredia);

        // 6. Voluntarios
        var juan = new Voluntario { Nombre = "Juan Pérez", Contacto = "juan.voluntario@refugio.cr | +506 8811-2233", Activo = true };
        var sofia = new Voluntario { Nombre = "Sofía Ramírez", Contacto = "sofia.voluntario@refugio.cr | +506 8844-5566", Activo = true };
        ctx.Voluntarios.AddRange(juan, sofia);

        await ctx.SaveChangesAsync(ct);

        // 7. Animales (matching Figma mockups + all lifecycle states)
        var luna = new Animal { Nombre = "Luna", RazaId = labrador.IdRaza, FechaNacimiento = ahora.AddYears(-4), Sexo = "Hembra", Tamanio = "Grande", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.Disponible };
        var caramelo = new Animal { Nombre = "Caramelo", RazaId = salchicha.IdRaza, FechaNacimiento = ahora.AddYears(-2), Sexo = "Macho", Tamanio = "Pequeño", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.Disponible };
        var toby = new Animal { Nombre = "Toby", RazaId = mestizo.IdRaza, FechaNacimiento = ahora.AddMonths(-18), Sexo = "Macho", Tamanio = "Mediano", Esterilizado = "No", Activo = true, Estado = EstadoAnimal.Disponible };
        var mia = new Animal { Nombre = "Mia", RazaId = criolloGato.IdRaza, FechaNacimiento = ahora.AddMonths(-3), Sexo = "Hembra", Tamanio = "Pequeño", Esterilizado = "No", Activo = true, Estado = EstadoAnimal.NoAdoptable };
        var canela = new Animal { Nombre = "Canela", RazaId = siames.IdRaza, FechaNacimiento = ahora.AddYears(-1), Sexo = "Hembra", Tamanio = "Pequeño", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.EnProceso };
        var luisito = new Animal { Nombre = "Luisito", RazaId = mestizo.IdRaza, FechaNacimiento = ahora.AddYears(-3), Sexo = "Macho", Tamanio = "Pequeño", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.EnSeguimiento };
        var miguelito = new Animal { Nombre = "Miguelito", RazaId = mestizo.IdRaza, FechaNacimiento = ahora.AddYears(-2), Sexo = "Macho", Tamanio = "Mediano", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.EnSeguimiento };
        var rocky = new Animal { Nombre = "Rocky", RazaId = labrador.IdRaza, FechaNacimiento = ahora.AddYears(-5), Sexo = "Macho", Tamanio = "Grande", Esterilizado = "Sí", Activo = true, Estado = EstadoAnimal.Adoptado };
        ctx.Animales.AddRange(luna, caramelo, toby, mia, canela, luisito, miguelito, rocky);

        // 8. Adoptantes
        var carlos = new Adoptante { Identificacion = "1-1234-5678", Nombre = "Carlos Ruiz", Contacto = "carlos@email.com | +506 8888-4444", OtrasMascotas = "Ninguna", TipoViviendaId = casaPatio.IdTipoVivienda, FechaRegistro = ahora.AddMonths(-2), Activo = true };
        var mariana = new Adoptante { Identificacion = "2-0987-6543", Nombre = "Mariana Soto", Contacto = "mariana@email.com | +506 7444-5555", OtrasMascotas = "1 gato esterilizado", TipoViviendaId = casaPatio.IdTipoVivienda, FechaRegistro = ahora.AddMonths(-3), Activo = true };
        var oscar = new Adoptante { Identificacion = "1-2345-6789", Nombre = "Oscar Gaitán", Contacto = "oscar@email.com | +506 2388-2211", OtrasMascotas = "Ninguna", TipoViviendaId = apartamento.IdTipoVivienda, FechaRegistro = ahora.AddMonths(-4), Activo = true };
        var valeria = new Adoptante { Identificacion = "4-0456-7890", Nombre = "Valeria Vargas", Contacto = "valeria@email.com | +506 6012-3344", OtrasMascotas = "1 perro pequeño", TipoViviendaId = casaSinPatio.IdTipoVivienda, FechaRegistro = ahora.AddMonths(-5), Activo = true };
        ctx.Adoptantes.AddRange(carlos, mariana, oscar, valeria);

        await ctx.SaveChangesAsync(ct);

        // 9. Vacunaciones (obligatorias al día para animales disponibles/en proceso/seguimiento)
        var animalesVacunados = new[] { luna, caramelo, toby, canela, luisito, miguelito, rocky };
        foreach (var animal in animalesVacunados)
        {
            ctx.Vacunaciones.AddRange(
                new Vacunacion { AnimalId = animal.IdAnimal, TipoVacunaId = rabia.IdTipoVacuna, Fecha = ahora.AddMonths(-2), ProximaDosis = ahora.AddMonths(10) },
                new Vacunacion { AnimalId = animal.IdAnimal, TipoVacunaId = parvovirus.IdTipoVacuna, Fecha = ahora.AddMonths(-2), ProximaDosis = ahora.AddMonths(10) },
                new Vacunacion { AnimalId = animal.IdAnimal, TipoVacunaId = moquillo.IdTipoVacuna, Fecha = ahora.AddMonths(-2), ProximaDosis = ahora.AddMonths(10) }
            );
        }
        // Vacuna próxima a vencer para probar alertas de vacunas
        ctx.Vacunaciones.Add(new Vacunacion { AnimalId = caramelo.IdAnimal, TipoVacunaId = bordetella.IdTipoVacuna, Fecha = ahora.AddMonths(-11), ProximaDosis = ahora.AddDays(5) });

        // 10. Consultas Veterinarias
        ctx.Consultas.AddRange(
            new ConsultaVeterinaria { AnimalId = luna.IdAnimal, VeterinariaId = vetSanJose.IdVeterinaria, Fecha = ahora.AddMonths(-3), Diagnostico = "Alergia leve en la piel del cuello", Tratamiento = "Antibióticos tópicos por 7 días; resuelto sin complicaciones" },
            new ConsultaVeterinaria { AnimalId = caramelo.IdAnimal, VeterinariaId = vetHeredia.IdVeterinaria, Fecha = ahora.AddMonths(-1), Diagnostico = "Chequeo de ingreso saludable (peso 7 kg)", Tratamiento = "Desparasitación interna y externa" },
            new ConsultaVeterinaria { AnimalId = mia.IdAnimal, VeterinariaId = vetSanJose.IdVeterinaria, Fecha = ahora.AddDays(-10), Diagnostico = "Bajo peso por rescate reciente", Tratamiento = "Dieta nutricional en tratamiento antes de habilitar para adopción" }
        );

        // 11. Solicitudes de Adopción
        var solCanela = new SolicitudAdopcion { AnimalId = canela.IdAnimal, AdoptanteId = carlos.IdAdoptante, FechaSolicitud = ahora.AddDays(-5), Estado = EstadoSolicitud.VisitaProgramada };
        var solLuisito = new SolicitudAdopcion { AnimalId = luisito.IdAnimal, AdoptanteId = mariana.IdAdoptante, FechaSolicitud = ahora.AddDays(-75), FechaResolucion = ahora.AddDays(-66), Estado = EstadoSolicitud.Aprobada };
        var solMiguelito = new SolicitudAdopcion { AnimalId = miguelito.IdAnimal, AdoptanteId = oscar.IdAdoptante, FechaSolicitud = ahora.AddDays(-100), FechaResolucion = ahora.AddDays(-92), Estado = EstadoSolicitud.Aprobada };
        var solRocky = new SolicitudAdopcion { AnimalId = rocky.IdAnimal, AdoptanteId = valeria.IdAdoptante, FechaSolicitud = ahora.AddDays(-130), FechaResolucion = ahora.AddDays(-120), Estado = EstadoSolicitud.Aprobada };
        ctx.Solicitudes.AddRange(solCanela, solLuisito, solMiguelito, solRocky);

        await ctx.SaveChangesAsync(ct);

        // 12. Visitas Domiciliarias
        ctx.Visitas.AddRange(
            new VisitaDomiciliaria { SolicitudId = solCanela.IdSolicitud, VoluntarioId = juan.IdVoluntario, Fecha = ahora.AddDays(2), Resultado = ResultadoVisita.Pendiente },
            new VisitaDomiciliaria { SolicitudId = solLuisito.IdSolicitud, VoluntarioId = juan.IdVoluntario, Fecha = ahora.AddDays(-70), Resultado = ResultadoVisita.Favorable, Observaciones = "Patio cerrado y condiciones óptimas." },
            new VisitaDomiciliaria { SolicitudId = solMiguelito.IdSolicitud, VoluntarioId = sofia.IdVoluntario, Fecha = ahora.AddDays(-95), Resultado = ResultadoVisita.Favorable, Observaciones = "Espacio seguro y ambiente tranquilo." },
            new VisitaDomiciliaria { SolicitudId = solRocky.IdSolicitud, VoluntarioId = juan.IdVoluntario, Fecha = ahora.AddDays(-125), Resultado = ResultadoVisita.Favorable, Observaciones = "Excelente disposición familiar." }
        );

        // 13. Seguimientos Post-Adopción (30, 60, 90 días)
        var segLuisito60 = new SeguimientoPostAdopcion { SolicitudId = solLuisito.IdSolicitud, Hito = HitoSeguimiento.Dias60, FechaProgramada = ahora.AddDays(-6), Estado = EstadoSeguimiento.Vencido };
        var segMiguelito90 = new SeguimientoPostAdopcion { SolicitudId = solMiguelito.IdSolicitud, Hito = HitoSeguimiento.Dias90, FechaProgramada = ahora.AddDays(-2), Estado = EstadoSeguimiento.Vencido };

        ctx.Seguimientos.AddRange(
            // Luisito
            new SeguimientoPostAdopcion { SolicitudId = solLuisito.IdSolicitud, Hito = HitoSeguimiento.Dias30, FechaProgramada = ahora.AddDays(-36), FechaRealizada = ahora.AddDays(-35), Estado = EstadoSeguimiento.Favorable, Observaciones = "Animal adaptado y saludable." },
            segLuisito60,
            new SeguimientoPostAdopcion { SolicitudId = solLuisito.IdSolicitud, Hito = HitoSeguimiento.Dias90, FechaProgramada = ahora.AddDays(24), Estado = EstadoSeguimiento.Pendiente },
            // Miguelito
            new SeguimientoPostAdopcion { SolicitudId = solMiguelito.IdSolicitud, Hito = HitoSeguimiento.Dias30, FechaProgramada = ahora.AddDays(-62), FechaRealizada = ahora.AddDays(-61), Estado = EstadoSeguimiento.Favorable, Observaciones = "Buen peso y energía." },
            new SeguimientoPostAdopcion { SolicitudId = solMiguelito.IdSolicitud, Hito = HitoSeguimiento.Dias60, FechaProgramada = ahora.AddDays(-32), FechaRealizada = ahora.AddDays(-31), Estado = EstadoSeguimiento.Favorable, Observaciones = "Se adaptó perfectamente a la familia." },
            segMiguelito90,
            // Rocky (adopción definitiva cerrada)
            new SeguimientoPostAdopcion { SolicitudId = solRocky.IdSolicitud, Hito = HitoSeguimiento.Dias30, FechaProgramada = ahora.AddDays(-90), FechaRealizada = ahora.AddDays(-89), Estado = EstadoSeguimiento.Favorable, Observaciones = "Excelente adaptación." },
            new SeguimientoPostAdopcion { SolicitudId = solRocky.IdSolicitud, Hito = HitoSeguimiento.Dias60, FechaProgramada = ahora.AddDays(-60), FechaRealizada = ahora.AddDays(-59), Estado = EstadoSeguimiento.Favorable, Observaciones = "Muy saludable." },
            new SeguimientoPostAdopcion { SolicitudId = solRocky.IdSolicitud, Hito = HitoSeguimiento.Dias90, FechaProgramada = ahora.AddDays(-30), FechaRealizada = ahora.AddDays(-29), Estado = EstadoSeguimiento.Favorable, Observaciones = "Adopción exitosa." }
        );

        await ctx.SaveChangesAsync(ct);

        // 14. Alertas de seguimiento vencido + Evento de cierre definitivo
        ctx.Alertas.AddRange(
            new AlertaSeguimiento
            {
                SeguimientoId = segLuisito60.IdSeguimiento,
                FechaCreacion = ahora.AddDays(-5),
                Mensaje = "Seguimiento de 60 días vencido sin realizar: Luisito (adoptante Mariana Soto).",
                EscaladaAlCoordinador = true,
                FechaEscalada = ahora.AddDays(-5)
            },
            new AlertaSeguimiento
            {
                SeguimientoId = segMiguelito90.IdSeguimiento,
                FechaCreacion = ahora.AddDays(-1),
                Mensaje = "Seguimiento de 90 días vencido sin realizar: Miguelito (adoptante Oscar Gaitán).",
                EscaladaAlCoordinador = true,
                FechaEscalada = ahora.AddDays(-1)
            }
        );

        ctx.Eventos.Add(new EventoAdopcion
        {
            SolicitudId = solRocky.IdSolicitud,
            AnimalId = rocky.IdAnimal,
            Tipo = TipoEventoAdopcion.AdopcionDefinitiva,
            Fecha = ahora.AddDays(-29),
            Detalle = "Seguimiento de 90 días favorable: adopción definitiva."
        });

        await ctx.SaveChangesAsync(ct);
        await BackfillDescripcionesYFotosAsync(ctx, ct);
    }

    private static async Task BackfillDescripcionesYFotosAsync(AdopcionContexto ctx, CancellationToken ct)
    {
        var datos = new Dictionary<string, (string FotoUrl, string Descripcion)>(StringComparer.OrdinalIgnoreCase)
        {
            ["Luna"] = (
                "https://placedog.net/800/600?id=15",
                "Luna espera una familia con la que empezar una nueva etapa. Incluso luego de ser encontrada en las calles de Guadalupe, es amigable con todos porque jamás negaría unas caricias."
            ),
            ["Caramelo"] = (
                "https://placedog.net/800/600?id=7",
                "Juguetón, curioso y parte de la camada de Canelita. Le encantan los paseos cortos, correr en el patio y dormir la siesta cerca de su familia."
            ),
            ["Toby"] = (
                "https://placedog.net/800/600?id=12",
                "Muy cariñoso y obediente. Aprende rápido con refuerzo positivo y disfruta las caminatas por el parque."
            ),
            ["Mia"] = (
                "https://cataas.com/cat?width=800&height=600&v=4",
                "Rescatada recientemente y en recuperación nutricional. Es dulce, tranquila y le gusta descansar en lugares cálidos."
            ),
            ["Canela"] = (
                "https://cataas.com/cat?width=800&height=600&v=5",
                "Gata siamés sociable, curiosa y muy limpia. Se adapta rápido a interiores tranquilos."
            ),
            ["Luisito"] = (
                "https://placedog.net/800/600?id=3",
                "Perro pequeño lleno de energía y muy apegado a su nueva familia."
            ),
            ["Miguelito"] = (
                "https://placedog.net/800/600?id=9",
                "Compañero fiel, tranquilo y muy amigable con visitas."
            ),
            ["Rocky"] = (
                "https://placedog.net/800/600?id=18",
                "Labrador noble y juguetón que ya disfruta de su hogar definitivo."
            ),
        };

        var sinDatos = await ctx.Animales
            .Where(a => a.Descripcion == null || a.FotoUrl == null)
            .ToListAsync(ct);

        var cambio = false;
        foreach (var animal in sinDatos)
        {
            if (datos.TryGetValue(animal.Nombre, out var info))
            {
                animal.FotoUrl ??= info.FotoUrl;
                animal.Descripcion ??= info.Descripcion;
                cambio = true;
            }
        }

        if (cambio)
            await ctx.SaveChangesAsync(ct);
    }
}
