using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFotoYDescripcionAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Animales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Animales",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Animales");
        }
    }
}
