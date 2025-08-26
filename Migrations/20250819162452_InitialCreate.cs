using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaByQr.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KullaniciAdi = table.Column<string>(type: "TEXT", nullable: false),
                    SifreHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    EtkinlikId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EtkinlikAdi = table.Column<string>(type: "TEXT", nullable: false),
                    QrCodeUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.EtkinlikId);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotograflar",
                columns: table => new
                {
                    FotografId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DosyaAdı = table.Column<string>(type: "TEXT", nullable: false),
                    DosyaYolu = table.Column<string>(type: "TEXT", nullable: false),
                    YuklemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EtkinlikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotograflar", x => x.FotografId);
                    table.ForeignKey(
                        name: "FK_Fotograflar_Etkinlikler_EtkinlikId",
                        column: x => x.EtkinlikId,
                        principalTable: "Etkinlikler",
                        principalColumn: "EtkinlikId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_KullaniciId",
                table: "Etkinlikler",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotograflar_EtkinlikId",
                table: "Fotograflar",
                column: "EtkinlikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotograflar");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
