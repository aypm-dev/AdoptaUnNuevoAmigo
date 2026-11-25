using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class ModeloCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEstado",
                table: "Especies");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Especies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Especies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Especies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Especies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Razas",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EspecieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Razas", x => x.IdRaza);
                    table.ForeignKey(
                        name: "FK_Razas_Especies_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especies",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TiposVacunas",
                columns: table => new
                {
                    IdTipoVacuna = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Obligatoria = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVacunas", x => x.IdTipoVacuna);
                });

            migrationBuilder.CreateTable(
                name: "TiposViviendas",
                columns: table => new
                {
                    ID_TIPO_VIVIENDA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposViviendas", x => x.ID_TIPO_VIVIENDA);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarias",
                columns: table => new
                {
                    IdVeterinaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Tratamiento = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarias", x => x.IdVeterinaria);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    IdVoluntario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.IdVoluntario);
                });

            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    IdAnimal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RazaId = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Tamanio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Esterilizado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animales", x => x.IdAnimal);
                    table.ForeignKey(
                        name: "FK_Animales_Razas_RazaId",
                        column: x => x.RazaId,
                        principalTable: "Razas",
                        principalColumn: "IdRaza",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Adoptantes",
                columns: table => new
                {
                    IdAdoptante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identificacion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OtrasMascotas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TipoViviendaId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adoptantes", x => x.IdAdoptante);
                    table.ForeignKey(
                        name: "FK_Adoptantes_TiposViviendas_TipoViviendaId",
                        column: x => x.TipoViviendaId,
                        principalTable: "TiposViviendas",
                        principalColumn: "ID_TIPO_VIVIENDA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultasVeterinarias",
                columns: table => new
                {
                    IdConsulta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeterinariaId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tratamiento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasVeterinarias", x => x.IdConsulta);
                    table.ForeignKey(
                        name: "FK_ConsultasVeterinarias_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsultasVeterinarias_Veterinarias_VeterinariaId",
                        column: x => x.VeterinariaId,
                        principalTable: "Veterinarias",
                        principalColumn: "IdVeterinaria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vacunaciones",
                columns: table => new
                {
                    IdVacunacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    TipoVacunaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProximaDosis = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacunaciones", x => x.IdVacunacion);
                    table.ForeignKey(
                        name: "FK_Vacunaciones_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vacunaciones_TiposVacunas_TipoVacunaId",
                        column: x => x.TipoVacunaId,
                        principalTable: "TiposVacunas",
                        principalColumn: "IdTipoVacuna",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesAdopcion",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    AdoptanteId = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaResolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAdopcion", x => x.IdSolicitud);
                    table.ForeignKey(
                        name: "FK_SolicitudesAdopcion_Adoptantes_AdoptanteId",
                        column: x => x.AdoptanteId,
                        principalTable: "Adoptantes",
                        principalColumn: "IdAdoptante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesAdopcion_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventosAdopcion",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(1100)", maxLength: 1100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosAdopcion", x => x.IdEvento);
                    table.ForeignKey(
                        name: "FK_EventosAdopcion_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventosAdopcion_SolicitudesAdopcion_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesAdopcion",
                        principalColumn: "IdSolicitud",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeguimientosPostAdopcion",
                columns: table => new
                {
                    IdSeguimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    Hito = table.Column<int>(type: "int", nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRealizada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosPostAdopcion", x => x.IdSeguimiento);
                    table.ForeignKey(
                        name: "FK_SeguimientosPostAdopcion_SolicitudesAdopcion_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesAdopcion",
                        principalColumn: "IdSolicitud",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitasDomiciliarias",
                columns: table => new
                {
                    IdVisita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    VoluntarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resultado = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasDomiciliarias", x => x.IdVisita);
                    table.ForeignKey(
                        name: "FK_VisitasDomiciliarias_SolicitudesAdopcion_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesAdopcion",
                        principalColumn: "IdSolicitud",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitasDomiciliarias_Voluntarios_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntarios",
                        principalColumn: "IdVoluntario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertasSeguimiento",
                columns: table => new
                {
                    IdAlerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeguimientoId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EscaladaAlCoordinador = table.Column<bool>(type: "bit", nullable: false),
                    FechaEscalada = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasSeguimiento", x => x.IdAlerta);
                    table.ForeignKey(
                        name: "FK_AlertasSeguimiento_SeguimientosPostAdopcion_SeguimientoId",
                        column: x => x.SeguimientoId,
                        principalTable: "SeguimientosPostAdopcion",
                        principalColumn: "IdSeguimiento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adoptantes_TipoViviendaId",
                table: "Adoptantes",
                column: "TipoViviendaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasSeguimiento_SeguimientoId",
                table: "AlertasSeguimiento",
                column: "SeguimientoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animales_RazaId",
                table: "Animales",
                column: "RazaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasVeterinarias_AnimalId_Fecha",
                table: "ConsultasVeterinarias",
                columns: new[] { "AnimalId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasVeterinarias_VeterinariaId",
                table: "ConsultasVeterinarias",
                column: "VeterinariaId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosAdopcion_AnimalId",
                table: "EventosAdopcion",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosAdopcion_SolicitudId",
                table: "EventosAdopcion",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Razas_EspecieId",
                table: "Razas",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Razas_FechaRegistro",
                table: "Razas",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_Estado_FechaProgramada",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "Estado", "FechaProgramada" });

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_SolicitudId_Hito",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "SolicitudId", "Hito" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAdopcion_AdoptanteId_Estado",
                table: "SolicitudesAdopcion",
                columns: new[] { "AdoptanteId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "UX_Solicitud_AnimalActiva",
                table: "SolicitudesAdopcion",
                column: "AnimalId",
                unique: true,
                filter: "[Estado] IN (0, 1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunaciones_AnimalId_TipoVacunaId_Fecha",
                table: "Vacunaciones",
                columns: new[] { "AnimalId", "TipoVacunaId", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacunaciones_TipoVacunaId",
                table: "Vacunaciones",
                column: "TipoVacunaId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasDomiciliarias_VoluntarioId",
                table: "VisitasDomiciliarias",
                column: "VoluntarioId");

            migrationBuilder.CreateIndex(
                name: "UX_Visita_PendientePorSolicitud",
                table: "VisitasDomiciliarias",
                column: "SolicitudId",
                unique: true,
                filter: "[Resultado] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasSeguimiento");

            migrationBuilder.DropTable(
                name: "ConsultasVeterinarias");

            migrationBuilder.DropTable(
                name: "EventosAdopcion");

            migrationBuilder.DropTable(
                name: "Vacunaciones");

            migrationBuilder.DropTable(
                name: "VisitasDomiciliarias");

            migrationBuilder.DropTable(
                name: "SeguimientosPostAdopcion");

            migrationBuilder.DropTable(
                name: "Veterinarias");

            migrationBuilder.DropTable(
                name: "TiposVacunas");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "SolicitudesAdopcion");

            migrationBuilder.DropTable(
                name: "Adoptantes");

            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "TiposViviendas");

            migrationBuilder.DropTable(
                name: "Razas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Especies");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Especies");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Especies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Especies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "IdEstado",
                table: "Especies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
