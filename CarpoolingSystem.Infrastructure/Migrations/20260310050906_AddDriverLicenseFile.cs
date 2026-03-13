using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverLicenseFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseFile",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseFileName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLicenseFile",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DriverLicenseFileName",
                table: "Users");
        }
    }
}
