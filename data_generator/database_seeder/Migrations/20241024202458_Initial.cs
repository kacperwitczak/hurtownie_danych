using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_seeder.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Krupierzy",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pesel = table.Column<long>(type: "bigint", nullable: false),
                    PoczatekPracy = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Krupierzy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lokalizacje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pietro = table.Column<short>(type: "smallint", nullable: false),
                    Rzad = table.Column<short>(type: "smallint", nullable: false),
                    Kolumna = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokalizacje", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TypGry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazwaGry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypGry", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TypTransakcji",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Typ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypTransakcji", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stoly",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_TypGryID = table.Column<int>(type: "int", nullable: false),
                    MinimalnaStawka = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaksymalnaStawka = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LiczbaMiejsc = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoly", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stoly_TypGry_FK_TypGryID",
                        column: x => x.FK_TypGryID,
                        principalTable: "TypGry",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UstawienieStolu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataKoniec = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FK_StolID = table.Column<int>(type: "int", nullable: false),
                    FK_LokalizacjaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UstawienieStolu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UstawienieStolu_Lokalizacje_FK_LokalizacjaID",
                        column: x => x.FK_LokalizacjaID,
                        principalTable: "Lokalizacje",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UstawienieStolu_Stoly_FK_StolID",
                        column: x => x.FK_StolID,
                        principalTable: "Stoly",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rozgrywki",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_KrupierID = table.Column<int>(type: "int", nullable: false),
                    FK_UstawienieStoluID = table.Column<int>(type: "int", nullable: false),
                    DataStart = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    DataKoniec = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rozgrywki", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rozgrywki_Krupierzy_FK_KrupierID",
                        column: x => x.FK_KrupierID,
                        principalTable: "Krupierzy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rozgrywki_UstawienieStolu_FK_UstawienieStoluID",
                        column: x => x.FK_UstawienieStoluID,
                        principalTable: "UstawienieStolu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transakcje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_RozgrywkaID = table.Column<int>(type: "int", nullable: false),
                    FK_TypTransakcjiID = table.Column<int>(type: "int", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcje", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transakcje_Rozgrywki_FK_RozgrywkaID",
                        column: x => x.FK_RozgrywkaID,
                        principalTable: "Rozgrywki",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transakcje_TypTransakcji_FK_TypTransakcjiID",
                        column: x => x.FK_TypTransakcjiID,
                        principalTable: "TypTransakcji",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rozgrywki_FK_KrupierID",
                table: "Rozgrywki",
                column: "FK_KrupierID");

            migrationBuilder.CreateIndex(
                name: "IX_Rozgrywki_FK_UstawienieStoluID",
                table: "Rozgrywki",
                column: "FK_UstawienieStoluID");

            migrationBuilder.CreateIndex(
                name: "IX_Stoly_FK_TypGryID",
                table: "Stoly",
                column: "FK_TypGryID");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcje_FK_RozgrywkaID",
                table: "Transakcje",
                column: "FK_RozgrywkaID");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcje_FK_TypTransakcjiID",
                table: "Transakcje",
                column: "FK_TypTransakcjiID");

            migrationBuilder.CreateIndex(
                name: "IX_UstawienieStolu_FK_LokalizacjaID",
                table: "UstawienieStolu",
                column: "FK_LokalizacjaID");

            migrationBuilder.CreateIndex(
                name: "IX_UstawienieStolu_FK_StolID",
                table: "UstawienieStolu",
                column: "FK_StolID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transakcje");

            migrationBuilder.DropTable(
                name: "Rozgrywki");

            migrationBuilder.DropTable(
                name: "TypTransakcji");

            migrationBuilder.DropTable(
                name: "Krupierzy");

            migrationBuilder.DropTable(
                name: "UstawienieStolu");

            migrationBuilder.DropTable(
                name: "Lokalizacje");

            migrationBuilder.DropTable(
                name: "Stoly");

            migrationBuilder.DropTable(
                name: "TypGry");
        }
    }
}
