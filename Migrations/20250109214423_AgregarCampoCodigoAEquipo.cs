using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoEquipos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoCodigoAEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Equipos");
        }
    }
}
