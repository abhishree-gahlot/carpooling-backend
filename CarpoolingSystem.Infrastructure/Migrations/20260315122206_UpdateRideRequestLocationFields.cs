using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRideRequestLocationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pickup",
                table: "RideRequests",
                newName: "PickupName");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "RideRequests",
                newName: "DestinationName");

            migrationBuilder.AddColumn<double>(
                name: "DestinationLatitude",
                table: "RideRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DestinationLongitude",
                table: "RideRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "RideRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "RideRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationLatitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "DestinationLongitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "PickupName",
                table: "RideRequests",
                newName: "Pickup");

            migrationBuilder.RenameColumn(
                name: "DestinationName",
                table: "RideRequests",
                newName: "Destination");
        }
    }
}
