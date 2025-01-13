using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoEquipos.Migrations
{
    /// <inheritdoc />
    public partial class CrearOrdenesDeTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Costo",
                table: "OrdenesTrabajo");

            migrationBuilder.RenameColumn(
                name: "TipoMantenimiento",
                table: "OrdenesTrabajo",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "Responsable",
                table: "OrdenesTrabajo",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "OrdenesTrabajo",
                newName: "FechaInicio");

            migrationBuilder.RenameColumn(
                name: "DescripcionTrabajo",
                table: "OrdenesTrabajo",
                newName: "Codigo");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "OrdenesTrabajo",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "OrdenesTrabajo");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "OrdenesTrabajo",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "OrdenesTrabajo",
                newName: "TipoMantenimiento");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "OrdenesTrabajo",
                newName: "Responsable");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "OrdenesTrabajo",
                newName: "DescripcionTrabajo");

            migrationBuilder.AddColumn<decimal>(
                name: "Costo",
                table: "OrdenesTrabajo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
