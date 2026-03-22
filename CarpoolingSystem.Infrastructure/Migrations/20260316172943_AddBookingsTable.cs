using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PIN = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BoardedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_RideRequests_RideRequestId",
                        column: x => x.RideRequestId,
                        principalTable: "RideRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_RideSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "RideSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RideRequestId",
                table: "Bookings",
                column: "RideRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId",
                table: "Bookings",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            //migrationBuilder.AddColumn<Guid>(
            //    name: "PassengerId",
            //    table: "RideSessions",
            //    type: "uniqueidentifier",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_RideSessions_PassengerId",
            //    table: "RideSessions",
            //    column: "PassengerId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_RideSessions_Users_PassengerId",
            //    table: "RideSessions",
            //    column: "PassengerId",
            //    principalTable: "Users",
            //    principalColumn: "UserId");
        }
    }
}
