using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToRunnerSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "RunnerSections",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RunnerSections_CategoryId",
                table: "RunnerSections",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RunnerSections_RaceCategory_CategoryId",
                table: "RunnerSections",
                column: "CategoryId",
                principalTable: "RaceCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunnerSections_RaceCategory_CategoryId",
                table: "RunnerSections");

            migrationBuilder.DropIndex(
                name: "IX_RunnerSections_CategoryId",
                table: "RunnerSections");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "RunnerSections");
        }
    }
}
