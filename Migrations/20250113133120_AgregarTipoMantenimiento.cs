using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoEquipos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTipoMantenimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoMantenimiento",
                table: "OrdenesTrabajo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoMantenimiento",
                table: "OrdenesTrabajo");
        }
    }
}
