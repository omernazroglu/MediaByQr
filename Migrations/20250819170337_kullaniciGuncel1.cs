using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaByQr.Migrations
{
    /// <inheritdoc />
    public partial class kullaniciGuncel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ad_Soyad",
                table: "Kullanicilar",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Kullanicilar",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad_Soyad",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Kullanicilar");
        }
    }
}
