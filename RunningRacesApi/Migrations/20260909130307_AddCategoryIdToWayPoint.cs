using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToWayPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "WayPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WayPoints_CategoryId",
                table: "WayPoints",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WayPoints_RaceCategory_CategoryId",
                table: "WayPoints",
                column: "CategoryId",
                principalTable: "RaceCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WayPoints_RaceCategory_CategoryId",
                table: "WayPoints");

            migrationBuilder.DropIndex(
                name: "IX_WayPoints_CategoryId",
                table: "WayPoints");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "WayPoints");
        }
    }
}
