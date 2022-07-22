using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMHO.Migrations
{
    public partial class updateAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTipoUsuario",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "RolUsuario",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AUViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimerNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegundoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegundoApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstadoUsuario = table.Column<int>(type: "int", nullable: false),
                    ListaRoles = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUViewModel");

            migrationBuilder.DropColumn(
                name: "RolUsuario",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "IdTipoUsuario",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
