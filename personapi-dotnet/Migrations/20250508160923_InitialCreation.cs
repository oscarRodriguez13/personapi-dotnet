using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personapi_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    cc = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    apellido = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    edad = table.Column<int>(type: "int", nullable: true),
                    genero = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__persona__3213666D564B0149", x => x.cc);
                });

            migrationBuilder.CreateTable(
                name: "profesion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nom = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    des = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__profesio__3213E83F2B1F13F8", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "telefono",
                columns: table => new
                {
                    num = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    oper = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    duenio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__telefono__DF908D659694C3B2", x => x.num);
                    table.ForeignKey(
                        name: "FK__telefono__duenio__37A5467C",
                        column: x => x.duenio,
                        principalTable: "persona",
                        principalColumn: "cc");
                });

            migrationBuilder.CreateTable(
                name: "estudios",
                columns: table => new
                {
                    id_prof = table.Column<int>(type: "int", nullable: false),
                    cc_per = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    univer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PersonaCc = table.Column<int>(type: "int", nullable: false),
                    ProfesionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__estudios__FB3F71A6B8574F61", x => new { x.id_prof, x.cc_per });
                    table.ForeignKey(
                        name: "FK__estudios__cc_per__33D4B598",
                        column: x => x.cc_per,
                        principalTable: "persona",
                        principalColumn: "cc");
                    table.ForeignKey(
                        name: "FK__estudios__id_pro__34C8D9D1",
                        column: x => x.id_prof,
                        principalTable: "profesion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_estudios_persona_PersonaCc",
                        column: x => x.PersonaCc,
                        principalTable: "persona",
                        principalColumn: "cc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_estudios_profesion_ProfesionId",
                        column: x => x.ProfesionId,
                        principalTable: "profesion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_estudios_cc_per",
                table: "estudios",
                column: "cc_per");

            migrationBuilder.CreateIndex(
                name: "IX_estudios_PersonaCc",
                table: "estudios",
                column: "PersonaCc");

            migrationBuilder.CreateIndex(
                name: "IX_estudios_ProfesionId",
                table: "estudios",
                column: "ProfesionId");

            migrationBuilder.CreateIndex(
                name: "IX_telefono_duenio",
                table: "telefono",
                column: "duenio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "estudios");

            migrationBuilder.DropTable(
                name: "telefono");

            migrationBuilder.DropTable(
                name: "profesion");

            migrationBuilder.DropTable(
                name: "persona");
        }
    }
}
