using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MultiPassengerBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RideRequests_RideRequestId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RideRequestId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BoardedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Fare",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PIN",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RideRequestId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "AcceptedAts",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BoardedAts",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompletedAts",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedAts",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EndedAts",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fares",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PINs",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RideRequestIds",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Statuses",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedAts",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BoardedAts",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CompletedAts",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAts",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EndedAts",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Fares",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PINs",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RideRequestIds",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Statuses",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BoardedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fare",
                table: "Bookings",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PIN",
                table: "Bookings",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RideRequestId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RideRequestId",
                table: "Bookings",
                column: "RideRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RideRequests_RideRequestId",
                table: "Bookings",
                column: "RideRequestId",
                principalTable: "RideRequests",
                principalColumn: "Id");
        }
    }
}
