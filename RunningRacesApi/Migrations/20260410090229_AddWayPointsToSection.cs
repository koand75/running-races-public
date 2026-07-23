using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddWayPointsToSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndWayPointId",
                table: "Sections",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartWayPointId",
                table: "Sections",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_EndWayPointId",
                table: "Sections",
                column: "EndWayPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_StartWayPointId",
                table: "Sections",
                column: "StartWayPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_WayPoints_EndWayPointId",
                table: "Sections",
                column: "EndWayPointId",
                principalTable: "WayPoints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_WayPoints_StartWayPointId",
                table: "Sections",
                column: "StartWayPointId",
                principalTable: "WayPoints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_WayPoints_EndWayPointId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_WayPoints_StartWayPointId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_EndWayPointId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_StartWayPointId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "EndWayPointId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StartWayPointId",
                table: "Sections");
        }
    }
}
