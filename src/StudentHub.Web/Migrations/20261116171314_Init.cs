using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LozinkaHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Uloga = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Biografija = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DatumRegistracije = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tagovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vjestine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vjestine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projekti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    VlasnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    KategorijaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    JeObjavljen = table.Column<bool>(type: "INTEGER", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumObjave = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BrojPregleda = table.Column<int>(type: "INTEGER", nullable: false),
                    ProsjecnaOcjena = table.Column<decimal>(type: "TEXT", precision: 3, scale: 2, nullable: true),
                    BrojOcjena = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projekti_Kategorije_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projekti_Korisnici_VlasnikId",
                        column: x => x.VlasnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikVjestine",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    VjestinaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikVjestine", x => new { x.KorisnikId, x.VjestinaId });
                    table.ForeignKey(
                        name: "FK_KorisnikVjestine_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikVjestine_Vjestine_VjestinaId",
                        column: x => x.VjestinaId,
                        principalTable: "Vjestine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClanoviTima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    Uloga = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DatumPridruzivanja = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanoviTima", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanoviTima_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClanoviTima_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbackovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Komentar = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Ocjena = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumAzuriranja = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbackovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbackovi_Korisnici_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbackovi_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oglasi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false),
                    NazivUloge = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    BrojMjesta = table.Column<int>(type: "INTEGER", nullable: false),
                    BrojSlobodnihMjesta = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DatumObjave = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumZatvaranja = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oglasi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oglasi_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjektTagovi",
                columns: table => new
                {
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjektTagovi", x => new { x.ProjektId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProjektTagovi_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjektTagovi_Tagovi_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdgovoriNaFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeedbackId = table.Column<int>(type: "INTEGER", nullable: false),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tekst = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdgovoriNaFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdgovoriNaFeedback_Feedbackovi_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbackovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdgovoriNaFeedback_Korisnici_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OglasVjestine",
                columns: table => new
                {
                    OglasId = table.Column<int>(type: "INTEGER", nullable: false),
                    VjestinaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasVjestine", x => new { x.OglasId, x.VjestinaId });
                    table.ForeignKey(
                        name: "FK_OglasVjestine_Oglasi_OglasId",
                        column: x => x.OglasId,
                        principalTable: "Oglasi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OglasVjestine_Vjestine_VjestinaId",
                        column: x => x.VjestinaId,
                        principalTable: "Vjestine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ponude",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OglasId = table.Column<int>(type: "INTEGER", nullable: false),
                    KandidatId = table.Column<int>(type: "INTEGER", nullable: false),
                    Poruka = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumOdluke = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponude", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ponude_Korisnici_KandidatId",
                        column: x => x.KandidatId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ponude_Oglasi_OglasId",
                        column: x => x.OglasId,
                        principalTable: "Oglasi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviTima_KorisnikId",
                table: "ClanoviTima",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviTima_ProjektId_KorisnikId",
                table: "ClanoviTima",
                columns: new[] { "ProjektId", "KorisnikId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbackovi_AutorId",
                table: "Feedbackovi",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbackovi_ProjektId_AutorId",
                table: "Feedbackovi",
                columns: new[] { "ProjektId", "AutorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kategorije_Naziv",
                table: "Kategorije",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_Email",
                table: "Korisnici",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikVjestine_VjestinaId",
                table: "KorisnikVjestine",
                column: "VjestinaId");

            migrationBuilder.CreateIndex(
                name: "IX_OdgovoriNaFeedback_AutorId",
                table: "OdgovoriNaFeedback",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_OdgovoriNaFeedback_FeedbackId",
                table: "OdgovoriNaFeedback",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Oglasi_ProjektId",
                table: "Oglasi",
                column: "ProjektId");

            migrationBuilder.CreateIndex(
                name: "IX_OglasVjestine_VjestinaId",
                table: "OglasVjestine",
                column: "VjestinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_KandidatId",
                table: "Ponude",
                column: "KandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_OglasId_KandidatId",
                table: "Ponude",
                columns: new[] { "OglasId", "KandidatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projekti_KategorijaId",
                table: "Projekti",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekti_VlasnikId",
                table: "Projekti",
                column: "VlasnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjektTagovi_TagId",
                table: "ProjektTagovi",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tagovi_Naziv",
                table: "Tagovi",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vjestine_Naziv",
                table: "Vjestine",
                column: "Naziv",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanoviTima");

            migrationBuilder.DropTable(
                name: "KorisnikVjestine");

            migrationBuilder.DropTable(
                name: "OdgovoriNaFeedback");

            migrationBuilder.DropTable(
                name: "OglasVjestine");

            migrationBuilder.DropTable(
                name: "Ponude");

            migrationBuilder.DropTable(
                name: "ProjektTagovi");

            migrationBuilder.DropTable(
                name: "Feedbackovi");

            migrationBuilder.DropTable(
                name: "Vjestine");

            migrationBuilder.DropTable(
                name: "Oglasi");

            migrationBuilder.DropTable(
                name: "Tagovi");

            migrationBuilder.DropTable(
                name: "Projekti");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
