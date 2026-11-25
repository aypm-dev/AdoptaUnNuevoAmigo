using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class AjustesFluentAPIVerificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptantes_TiposViviendas_TipoViviendaId",
                table: "Adoptantes");

            migrationBuilder.AlterColumn<string>(
                name: "Tratamiento",
                table: "Veterinarias",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnostico",
                table: "Veterinarias",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPCION",
                table: "TiposViviendas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.DropIndex(
                name: "IX_SeguimientosPostAdopcion_SolicitudId_Hito",
                table: "SeguimientosPostAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_SeguimientosPostAdopcion_Estado_FechaProgramada",
                table: "SeguimientosPostAdopcion");

            migrationBuilder.AlterColumn<string>(
                name: "Hito",
                table: "SeguimientosPostAdopcion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "SeguimientosPostAdopcion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("""
                UPDATE SeguimientosPostAdopcion
                SET Hito = CASE Hito
                    WHEN '30' THEN 'Dias30'
                    WHEN '60' THEN 'Dias60'
                    WHEN '90' THEN 'Dias90'
                    ELSE Hito END,
                    Estado = CASE Estado
                    WHEN '0' THEN 'Pendiente'
                    WHEN '1' THEN 'Favorable'
                    WHEN '2' THEN 'Desfavorable'
                    WHEN '3' THEN 'Vencido'
                    WHEN '4' THEN 'Omitido'
                    ELSE Estado END;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_SolicitudId_Hito",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "SolicitudId", "Hito" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_Estado_FechaProgramada",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "Estado", "FechaProgramada" });

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "EventosAdopcion",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("""
                UPDATE EventosAdopcion
                SET Tipo = CASE Tipo
                    WHEN '0' THEN 'AdopcionDefinitiva'
                    WHEN '1' THEN 'AdopcionFallida'
                    ELSE Tipo END;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Especies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Disponible",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE Animales
                SET Estado = CASE Estado
                    WHEN '0' THEN 'Disponible'
                    WHEN '1' THEN 'EnProceso'
                    WHEN '2' THEN 'EnSeguimiento'
                    WHEN '3' THEN 'Adoptado'
                    WHEN '4' THEN 'NoAdoptable'
                    ELSE Estado END;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Contacto",
                table: "Adoptantes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_TiposViviendas_NOMBRE",
                table: "TiposViviendas",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposVacunas_Nombre",
                table: "TiposVacunas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Razas_EspecieId_Nombre",
                table: "Razas",
                columns: new[] { "EspecieId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Especies_Nombre",
                table: "Especies",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adoptantes_Identificacion",
                table: "Adoptantes",
                column: "Identificacion",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptantes_TiposViviendas_TipoViviendaId",
                table: "Adoptantes",
                column: "TipoViviendaId",
                principalTable: "TiposViviendas",
                principalColumn: "ID_TIPO_VIVIENDA",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptantes_TiposViviendas_TipoViviendaId",
                table: "Adoptantes");

            migrationBuilder.DropIndex(
                name: "IX_TiposViviendas_NOMBRE",
                table: "TiposViviendas");

            migrationBuilder.DropIndex(
                name: "IX_TiposVacunas_Nombre",
                table: "TiposVacunas");

            migrationBuilder.DropIndex(
                name: "IX_Razas_EspecieId_Nombre",
                table: "Razas");

            migrationBuilder.DropIndex(
                name: "IX_Especies_Nombre",
                table: "Especies");

            migrationBuilder.DropIndex(
                name: "IX_Adoptantes_Identificacion",
                table: "Adoptantes");

            migrationBuilder.AlterColumn<string>(
                name: "Tratamiento",
                table: "Veterinarias",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnostico",
                table: "Veterinarias",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPCION",
                table: "TiposViviendas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.DropIndex(
                name: "IX_SeguimientosPostAdopcion_SolicitudId_Hito",
                table: "SeguimientosPostAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_SeguimientosPostAdopcion_Estado_FechaProgramada",
                table: "SeguimientosPostAdopcion");

            migrationBuilder.AlterColumn<int>(
                name: "Hito",
                table: "SeguimientosPostAdopcion",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "SeguimientosPostAdopcion",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_SolicitudId_Hito",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "SolicitudId", "Hito" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosPostAdopcion_Estado_FechaProgramada",
                table: "SeguimientosPostAdopcion",
                columns: new[] { "Estado", "FechaProgramada" });

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "EventosAdopcion",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Especies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Animales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Disponible");

            migrationBuilder.AlterColumn<string>(
                name: "Contacto",
                table: "Adoptantes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptantes_TiposViviendas_TipoViviendaId",
                table: "Adoptantes",
                column: "TipoViviendaId",
                principalTable: "TiposViviendas",
                principalColumn: "ID_TIPO_VIVIENDA",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
