using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoEquipos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposFaltantesAEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Equipos",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "CostoAproximado",
                table: "Equipos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FrecuenciaMantenimiento",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VidaUtil",
                table: "Equipos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostoAproximado",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "FrecuenciaMantenimiento",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "VidaUtil",
                table: "Equipos");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);
        }
    }
}
