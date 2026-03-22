using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverHistoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverHistories",
                columns: table => new
                {
                    DriverHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DestinationLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropOffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalFare = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverHistories", x => x.DriverHistoryId);
                    table.ForeignKey(
                        name: "FK_DriverHistories_RideSessions_RideSessionId",
                        column: x => x.RideSessionId,
                        principalTable: "RideSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DriverHistories_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DriverHistoryPassengers",
                columns: table => new
                {
                    PassengerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Pickup = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    PickupTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ratings = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverHistoryPassengers", x => x.PassengerHistoryId);
                    table.ForeignKey(
                        name: "FK_DriverHistoryPassengers_DriverHistories_DriverHistoryId",
                        column: x => x.DriverHistoryId,
                        principalTable: "DriverHistories",
                        principalColumn: "DriverHistoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverHistoryPassengers_RideRequests_RideId",
                        column: x => x.RideId,
                        principalTable: "RideRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverHistories_DriverId",
                table: "DriverHistories",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverHistories_RideSessionId",
                table: "DriverHistories",
                column: "RideSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverHistoryPassengers_DriverHistoryId",
                table: "DriverHistoryPassengers",
                column: "DriverHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverHistoryPassengers_RideId",
                table: "DriverHistoryPassengers",
                column: "RideId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverHistoryPassengers");

            migrationBuilder.DropTable(
                name: "DriverHistories");
        }
    }
}
