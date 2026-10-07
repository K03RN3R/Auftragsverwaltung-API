using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GUI_2.Migrations
{
    /// <inheritdoc />
    public partial class modelsundconfigsumnullableergaenzt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kunden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Strasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hausnummer = table.Column<int>(type: "int", nullable: false),
                    PLZ = table.Column<int>(type: "int", nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunden", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stati", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anlagen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnlageGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AnlagenCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anlagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anlagen_Kunden_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schichten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchichtGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartzeitTag = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndzeitTag = table.Column<TimeOnly>(type: "time", nullable: false),
                    Mitternachtsarbeit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schichten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schichten_Kunden_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auftraege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuftragGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    AnlageId = table.Column<int>(type: "int", nullable: false),
                    Auftragsname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Auftragstyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MengeSOLL = table.Column<int>(type: "int", nullable: false),
                    MengeIO = table.Column<int>(type: "int", nullable: false),
                    MengeNIO = table.Column<int>(type: "int", nullable: false),
                    TaktZiel = table.Column<double>(type: "float", nullable: false),
                    TaktBerechnet = table.Column<double>(type: "float", nullable: false),
                    StartzeitUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndzeitUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Prioritaet = table.Column<int>(type: "int", nullable: false),
                    Quelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Beladen = table.Column<bool>(type: "bit", nullable: false),
                    BeladenAnzahl = table.Column<int>(type: "int", nullable: false),
                    Bilddaten = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auftraege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auftraege_Anlagen_AnlageId",
                        column: x => x.AnlageId,
                        principalTable: "Anlagen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auftraege_Kunden_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auftraege_Stati_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Stati",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stationen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AnlageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stationen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stationen_Anlagen_AnlageId",
                        column: x => x.AnlageId,
                        principalTable: "Anlagen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuftragSchichten",
                columns: table => new
                {
                    AuftragId = table.Column<int>(type: "int", nullable: false),
                    SchichtId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuftragSchichten", x => new { x.AuftragId, x.SchichtId });
                    table.ForeignKey(
                        name: "FK_AuftragSchichten_Auftraege_AuftragId",
                        column: x => x.AuftragId,
                        principalTable: "Auftraege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuftragSchichten_Schichten_SchichtId",
                        column: x => x.SchichtId,
                        principalTable: "Schichten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Spindeln",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpindelGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    Nummer = table.Column<int>(type: "int", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spindeln", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spindeln_Stationen_StationId",
                        column: x => x.StationId,
                        principalTable: "Stationen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fehlerberichte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FehlerberichtGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    AuftragId = table.Column<int>(type: "int", nullable: false),
                    SpindelId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Spindelnummer = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Beschreibung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Schwere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AufgetretenUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BestaetigtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BehobenUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErstelltUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fehlerberichte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fehlerberichte_Auftraege_AuftragId",
                        column: x => x.AuftragId,
                        principalTable: "Auftraege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fehlerberichte_Spindeln_SpindelId",
                        column: x => x.SpindelId,
                        principalTable: "Spindeln",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fehlerberichte_Stationen_StationId",
                        column: x => x.StationId,
                        principalTable: "Stationen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anlagen_KundeId",
                table: "Anlagen",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Auftraege_AnlageId",
                table: "Auftraege",
                column: "AnlageId");

            migrationBuilder.CreateIndex(
                name: "IX_Auftraege_KundeId",
                table: "Auftraege",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Auftraege_StatusId",
                table: "Auftraege",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuftragSchichten_SchichtId",
                table: "AuftragSchichten",
                column: "SchichtId");

            migrationBuilder.CreateIndex(
                name: "IX_Fehlerberichte_AuftragId",
                table: "Fehlerberichte",
                column: "AuftragId");

            migrationBuilder.CreateIndex(
                name: "IX_Fehlerberichte_SpindelId",
                table: "Fehlerberichte",
                column: "SpindelId");

            migrationBuilder.CreateIndex(
                name: "IX_Fehlerberichte_StationId",
                table: "Fehlerberichte",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Schichten_KundeId",
                table: "Schichten",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Spindeln_StationId",
                table: "Spindeln",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stationen_AnlageId",
                table: "Stationen",
                column: "AnlageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuftragSchichten");

            migrationBuilder.DropTable(
                name: "Fehlerberichte");

            migrationBuilder.DropTable(
                name: "Schichten");

            migrationBuilder.DropTable(
                name: "Auftraege");

            migrationBuilder.DropTable(
                name: "Spindeln");

            migrationBuilder.DropTable(
                name: "Stati");

            migrationBuilder.DropTable(
                name: "Stationen");

            migrationBuilder.DropTable(
                name: "Anlagen");

            migrationBuilder.DropTable(
                name: "Kunden");
        }
    }
}
