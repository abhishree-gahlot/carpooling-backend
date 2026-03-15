using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRideSessionIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideSessions_Users_PassengerId",
                table: "RideSessions");

            migrationBuilder.DropIndex(
                name: "IX_RideSessions_PassengerId",
                table: "RideSessions");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "RideSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PassengerId",
                table: "RideSessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideSessions_PassengerId",
                table: "RideSessions",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideSessions_Users_PassengerId",
                table: "RideSessions",
                column: "PassengerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
