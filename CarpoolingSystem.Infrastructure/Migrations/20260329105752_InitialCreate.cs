using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverLicenseFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverLicenseFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RideRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickupLatitude = table.Column<double>(type: "float", nullable: false),
                    PickupLongitude = table.Column<double>(type: "float", nullable: false),
                    PickupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    DestinationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RideRequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideRequests_Users_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxSeats = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickupLatitude = table.Column<double>(type: "float", nullable: false),
                    PickupLongitude = table.Column<double>(type: "float", nullable: false),
                    PickupName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    DestinationName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriverAvailability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideSessions_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_RideSessions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideRequestIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PINs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fares = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statuses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptedAts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoardedAts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletedAts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndedAts = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_RideSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "RideSessions",
                        principalColumn: "Id");
                });

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
                name: "IX_Bookings_SessionId",
                table: "Bookings",
                column: "SessionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_PassengerId",
                table: "RideRequests",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_RideSessions_DriverActive",
                table: "RideSessions",
                columns: new[] { "DriverId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RideSessions_VehicleId",
                table: "RideSessions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "DriverHistoryPassengers");

            migrationBuilder.DropTable(
                name: "DriverHistories");

            migrationBuilder.DropTable(
                name: "RideRequests");

            migrationBuilder.DropTable(
                name: "RideSessions");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
