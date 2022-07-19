using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMHO.Migrations
{
    public partial class updateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEstadoEmpleado",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IdTipoEmpleado",
                table: "User",
                newName: "IdTipoUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTipoUsuario",
                table: "User",
                newName: "IdTipoEmpleado");

            migrationBuilder.AddColumn<int>(
                name: "IdEstadoEmpleado",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
