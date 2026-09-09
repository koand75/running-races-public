using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRaceIdFromWayPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WayPoints_Races_RaceId",
                table: "WayPoints");

            migrationBuilder.DropIndex(
                name: "IX_WayPoints_RaceId",
                table: "WayPoints");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "WayPoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "WayPoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WayPoints_RaceId",
                table: "WayPoints",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_WayPoints_Races_RaceId",
                table: "WayPoints",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
