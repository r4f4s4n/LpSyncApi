using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LpSync.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuaria",
                columns: table => new
                {
                    DiscogsUsername = table.Column<string>(nullable: false),
                    EsHabilitada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuaria", x => x.DiscogsUsername);
                });

            migrationBuilder.CreateTable(
                name: "Acceso",
                columns: table => new
                {
                    AccesoID = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    UsuariaDiscogsUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acceso", x => x.AccesoID);
                    table.ForeignKey(
                        name: "FK_Acceso_Usuaria_UsuariaDiscogsUsername",
                        column: x => x.UsuariaDiscogsUsername,
                        principalTable: "Usuaria",
                        principalColumn: "DiscogsUsername",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acceso_UsuariaDiscogsUsername",
                table: "Acceso",
                column: "UsuariaDiscogsUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acceso");

            migrationBuilder.DropTable(
                name: "Usuaria");
        }
    }
}
