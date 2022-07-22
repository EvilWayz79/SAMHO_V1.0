using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMHO.Migrations
{
    public partial class adicionarMedicinas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoUsuario",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "AUViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "AUViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdEspecialidad",
                table: "AUViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdHorarioTrabajo",
                table: "AUViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPaisNacimiento",
                table: "AUViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Identificacion",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaisNacimiento",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoIdentificacion",
                table: "AUViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Medicina",
                columns: table => new
                {
                    IdMedicina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioMedicina = table.Column<double>(type: "float", nullable: false),
                    IdEstadoMedicina = table.Column<int>(type: "int", nullable: false),
                    CantidadMedicina = table.Column<int>(type: "int", nullable: false),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicina", x => x.IdMedicina);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicina");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "EstadoUsuario",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "IdEspecialidad",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "IdHorarioTrabajo",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "IdPaisNacimiento",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "Identificacion",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "PaisNacimiento",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "TipoIdentificacion",
                table: "AUViewModel");
        }
    }
}
