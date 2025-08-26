using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaByQr.Migrations
{
    /// <inheritdoc />
    public partial class etkinlikGuncel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeKey",
                table: "Etkinlikler",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeKey",
                table: "Etkinlikler");
        }
    }
}
