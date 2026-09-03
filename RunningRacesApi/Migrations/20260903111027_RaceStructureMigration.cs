using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RaceStructureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "WayPoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "Sections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RaceId",
                table: "RunnerSections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RaceType",
                table: "Races",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RaceTeam",
                columns: table => new
                {
                    RacesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceTeam", x => new { x.RacesId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_RaceTeam_Races_RacesId",
                        column: x => x.RacesId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RaceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "RaceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "RaceType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "RaceType",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WayPoints_RaceId",
                table: "WayPoints",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RaceId",
                table: "Sections",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RunnerSections_RaceId",
                table: "RunnerSections",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceTeam_TeamsId",
                table: "RaceTeam",
                column: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RunnerSections_Races_RaceId",
                table: "RunnerSections",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Races_RaceId",
                table: "Sections",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WayPoints_Races_RaceId",
                table: "WayPoints",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunnerSections_Races_RaceId",
                table: "RunnerSections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Races_RaceId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_WayPoints_Races_RaceId",
                table: "WayPoints");

            migrationBuilder.DropTable(
                name: "RaceTeam");

            migrationBuilder.DropIndex(
                name: "IX_WayPoints_RaceId",
                table: "WayPoints");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RaceId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_RunnerSections_RaceId",
                table: "RunnerSections");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "WayPoints");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "RunnerSections");

            migrationBuilder.DropColumn(
                name: "RaceType",
                table: "Races");
        }
    }
}
