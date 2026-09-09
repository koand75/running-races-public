using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRaceIdFromSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Races_RaceId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Races_RaceId1",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RaceId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RaceId1",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RaceId1",
                table: "Sections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "Sections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RaceId1",
                table: "Sections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RaceId",
                table: "Sections",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RaceId1",
                table: "Sections",
                column: "RaceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Races_RaceId",
                table: "Sections",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Races_RaceId1",
                table: "Sections",
                column: "RaceId1",
                principalTable: "Races",
                principalColumn: "Id");
        }
    }
}
