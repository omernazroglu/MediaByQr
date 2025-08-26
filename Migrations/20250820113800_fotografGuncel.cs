using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaByQr.Migrations
{
    /// <inheritdoc />
    public partial class fotografGuncel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DosyaTipi",
                table: "Fotograflar",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaTipi",
                table: "Fotograflar");
        }
    }
}
