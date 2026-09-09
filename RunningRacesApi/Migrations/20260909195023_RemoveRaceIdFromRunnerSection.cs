using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRaceIdFromRunnerSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunnerSections_Races_RaceId",
                table: "RunnerSections");

            migrationBuilder.DropIndex(
                name: "IX_RunnerSections_RaceId",
                table: "RunnerSections");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "RunnerSections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "RunnerSections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RunnerSections_RaceId",
                table: "RunnerSections",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RunnerSections_Races_RaceId",
                table: "RunnerSections",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
