using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStartEndPointFromSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_WayPoints_EndWayPointId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_WayPoints_StartWayPointId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "EndPoint",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "StartPoint",
                table: "Sections");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_WayPoints_EndWayPointId",
                table: "Sections",
                column: "EndWayPointId",
                principalTable: "WayPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_WayPoints_StartWayPointId",
                table: "Sections",
                column: "StartWayPointId",
                principalTable: "WayPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AddColumn<string>(
                name: "EndPoint",
                table: "Sections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartPoint",
                table: "Sections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
    }
}
