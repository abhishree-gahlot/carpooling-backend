using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRideSessionLocationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dropoff",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "Pickup",
                table: "RideSessions");

            migrationBuilder.AddColumn<double>(
                name: "DestinationLatitude",
                table: "RideSessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DestinationLongitude",
                table: "RideSessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DestinationName",
                table: "RideSessions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "RideSessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "RideSessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PickupName",
                table: "RideSessions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationLatitude",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "DestinationLongitude",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "DestinationName",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "PickupName",
                table: "RideSessions");

            migrationBuilder.AddColumn<string>(
                name: "Dropoff",
                table: "RideSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pickup",
                table: "RideSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
