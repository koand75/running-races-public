using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceCategoryTeamExplicit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceCategoryTeam");

            migrationBuilder.CreateTable(
                name: "RaceCategoryTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceCategoryTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceCategoryTeams_RaceCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RaceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceCategoryTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceCategoryTeams_CategoryId",
                table: "RaceCategoryTeams",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceCategoryTeams_TeamId",
                table: "RaceCategoryTeams",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceCategoryTeams");

            migrationBuilder.CreateTable(
                name: "RaceCategoryTeam",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceCategoryTeam", x => new { x.CategoriesId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_RaceCategoryTeam_RaceCategory_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "RaceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceCategoryTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceCategoryTeam_TeamsId",
                table: "RaceCategoryTeam",
                column: "TeamsId");
        }
    }
}
