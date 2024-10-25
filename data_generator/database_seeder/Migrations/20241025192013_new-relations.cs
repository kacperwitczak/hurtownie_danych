using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_seeder.Migrations
{
    /// <inheritdoc />
    public partial class newrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rozgrywki_Krupierzy_FK_KrupierID",
                table: "Rozgrywki");

            migrationBuilder.DropForeignKey(
                name: "FK_Rozgrywki_UstawienieStolu_FK_UstawienieStoluID",
                table: "Rozgrywki");

            migrationBuilder.DropForeignKey(
                name: "FK_Stoly_TypGry_FK_TypGryID",
                table: "Stoly");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcje_Rozgrywki_FK_RozgrywkaID",
                table: "Transakcje");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcje_TypTransakcji_FK_TypTransakcjiID",
                table: "Transakcje");

            migrationBuilder.DropForeignKey(
                name: "FK_UstawienieStolu_Lokalizacje_FK_LokalizacjaID",
                table: "UstawienieStolu");

            migrationBuilder.DropForeignKey(
                name: "FK_UstawienieStolu_Stoly_FK_StolID",
                table: "UstawienieStolu");

            migrationBuilder.RenameColumn(
                name: "FK_StolID",
                table: "UstawienieStolu",
                newName: "StolyID");

            migrationBuilder.RenameColumn(
                name: "FK_LokalizacjaID",
                table: "UstawienieStolu",
                newName: "LokalizacjeID");

            migrationBuilder.RenameIndex(
                name: "IX_UstawienieStolu_FK_StolID",
                table: "UstawienieStolu",
                newName: "IX_UstawienieStolu_StolyID");

            migrationBuilder.RenameIndex(
                name: "IX_UstawienieStolu_FK_LokalizacjaID",
                table: "UstawienieStolu",
                newName: "IX_UstawienieStolu_LokalizacjeID");

            migrationBuilder.RenameColumn(
                name: "FK_TypTransakcjiID",
                table: "Transakcje",
                newName: "TypTransakcjiID");

            migrationBuilder.RenameColumn(
                name: "FK_RozgrywkaID",
                table: "Transakcje",
                newName: "RozgrywkiID");

            migrationBuilder.RenameIndex(
                name: "IX_Transakcje_FK_TypTransakcjiID",
                table: "Transakcje",
                newName: "IX_Transakcje_TypTransakcjiID");

            migrationBuilder.RenameIndex(
                name: "IX_Transakcje_FK_RozgrywkaID",
                table: "Transakcje",
                newName: "IX_Transakcje_RozgrywkiID");

            migrationBuilder.RenameColumn(
                name: "FK_TypGryID",
                table: "Stoly",
                newName: "TypGryID");

            migrationBuilder.RenameIndex(
                name: "IX_Stoly_FK_TypGryID",
                table: "Stoly",
                newName: "IX_Stoly_TypGryID");

            migrationBuilder.RenameColumn(
                name: "FK_UstawienieStoluID",
                table: "Rozgrywki",
                newName: "UstawienieStoluID");

            migrationBuilder.RenameColumn(
                name: "FK_KrupierID",
                table: "Rozgrywki",
                newName: "KrupierID");

            migrationBuilder.RenameIndex(
                name: "IX_Rozgrywki_FK_UstawienieStoluID",
                table: "Rozgrywki",
                newName: "IX_Rozgrywki_UstawienieStoluID");

            migrationBuilder.RenameIndex(
                name: "IX_Rozgrywki_FK_KrupierID",
                table: "Rozgrywki",
                newName: "IX_Rozgrywki_KrupierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rozgrywki_Krupierzy_KrupierID",
                table: "Rozgrywki",
                column: "KrupierID",
                principalTable: "Krupierzy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rozgrywki_UstawienieStolu_UstawienieStoluID",
                table: "Rozgrywki",
                column: "UstawienieStoluID",
                principalTable: "UstawienieStolu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stoly_TypGry_TypGryID",
                table: "Stoly",
                column: "TypGryID",
                principalTable: "TypGry",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcje_Rozgrywki_RozgrywkiID",
                table: "Transakcje",
                column: "RozgrywkiID",
                principalTable: "Rozgrywki",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcje_TypTransakcji_TypTransakcjiID",
                table: "Transakcje",
                column: "TypTransakcjiID",
                principalTable: "TypTransakcji",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UstawienieStolu_Lokalizacje_LokalizacjeID",
                table: "UstawienieStolu",
                column: "LokalizacjeID",
                principalTable: "Lokalizacje",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UstawienieStolu_Stoly_StolyID",
                table: "UstawienieStolu",
                column: "StolyID",
                principalTable: "Stoly",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rozgrywki_Krupierzy_KrupierID",
                table: "Rozgrywki");

            migrationBuilder.DropForeignKey(
                name: "FK_Rozgrywki_UstawienieStolu_UstawienieStoluID",
                table: "Rozgrywki");

            migrationBuilder.DropForeignKey(
                name: "FK_Stoly_TypGry_TypGryID",
                table: "Stoly");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcje_Rozgrywki_RozgrywkiID",
                table: "Transakcje");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcje_TypTransakcji_TypTransakcjiID",
                table: "Transakcje");

            migrationBuilder.DropForeignKey(
                name: "FK_UstawienieStolu_Lokalizacje_LokalizacjeID",
                table: "UstawienieStolu");

            migrationBuilder.DropForeignKey(
                name: "FK_UstawienieStolu_Stoly_StolyID",
                table: "UstawienieStolu");

            migrationBuilder.RenameColumn(
                name: "StolyID",
                table: "UstawienieStolu",
                newName: "FK_StolID");

            migrationBuilder.RenameColumn(
                name: "LokalizacjeID",
                table: "UstawienieStolu",
                newName: "FK_LokalizacjaID");

            migrationBuilder.RenameIndex(
                name: "IX_UstawienieStolu_StolyID",
                table: "UstawienieStolu",
                newName: "IX_UstawienieStolu_FK_StolID");

            migrationBuilder.RenameIndex(
                name: "IX_UstawienieStolu_LokalizacjeID",
                table: "UstawienieStolu",
                newName: "IX_UstawienieStolu_FK_LokalizacjaID");

            migrationBuilder.RenameColumn(
                name: "TypTransakcjiID",
                table: "Transakcje",
                newName: "FK_TypTransakcjiID");

            migrationBuilder.RenameColumn(
                name: "RozgrywkiID",
                table: "Transakcje",
                newName: "FK_RozgrywkaID");

            migrationBuilder.RenameIndex(
                name: "IX_Transakcje_TypTransakcjiID",
                table: "Transakcje",
                newName: "IX_Transakcje_FK_TypTransakcjiID");

            migrationBuilder.RenameIndex(
                name: "IX_Transakcje_RozgrywkiID",
                table: "Transakcje",
                newName: "IX_Transakcje_FK_RozgrywkaID");

            migrationBuilder.RenameColumn(
                name: "TypGryID",
                table: "Stoly",
                newName: "FK_TypGryID");

            migrationBuilder.RenameIndex(
                name: "IX_Stoly_TypGryID",
                table: "Stoly",
                newName: "IX_Stoly_FK_TypGryID");

            migrationBuilder.RenameColumn(
                name: "UstawienieStoluID",
                table: "Rozgrywki",
                newName: "FK_UstawienieStoluID");

            migrationBuilder.RenameColumn(
                name: "KrupierID",
                table: "Rozgrywki",
                newName: "FK_KrupierID");

            migrationBuilder.RenameIndex(
                name: "IX_Rozgrywki_UstawienieStoluID",
                table: "Rozgrywki",
                newName: "IX_Rozgrywki_FK_UstawienieStoluID");

            migrationBuilder.RenameIndex(
                name: "IX_Rozgrywki_KrupierID",
                table: "Rozgrywki",
                newName: "IX_Rozgrywki_FK_KrupierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rozgrywki_Krupierzy_FK_KrupierID",
                table: "Rozgrywki",
                column: "FK_KrupierID",
                principalTable: "Krupierzy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rozgrywki_UstawienieStolu_FK_UstawienieStoluID",
                table: "Rozgrywki",
                column: "FK_UstawienieStoluID",
                principalTable: "UstawienieStolu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stoly_TypGry_FK_TypGryID",
                table: "Stoly",
                column: "FK_TypGryID",
                principalTable: "TypGry",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcje_Rozgrywki_FK_RozgrywkaID",
                table: "Transakcje",
                column: "FK_RozgrywkaID",
                principalTable: "Rozgrywki",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcje_TypTransakcji_FK_TypTransakcjiID",
                table: "Transakcje",
                column: "FK_TypTransakcjiID",
                principalTable: "TypTransakcji",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UstawienieStolu_Lokalizacje_FK_LokalizacjaID",
                table: "UstawienieStolu",
                column: "FK_LokalizacjaID",
                principalTable: "Lokalizacje",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UstawienieStolu_Stoly_FK_StolID",
                table: "UstawienieStolu",
                column: "FK_StolID",
                principalTable: "Stoly",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
