using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoEquipos.Migrations
{
    /// <inheritdoc />
    public partial class EliminarEquipoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "OrdenesTrabajo");

            migrationBuilder.AddColumn<string>(
                name: "NumeroOrden",
                table: "OrdenesTrabajo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroOrden",
                table: "OrdenesTrabajo");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "OrdenesTrabajo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
