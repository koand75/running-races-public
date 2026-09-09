using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Sections",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CategoryId",
                table: "Sections",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_RaceCategory_CategoryId",
                table: "Sections",
                column: "CategoryId",
                principalTable: "RaceCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_RaceCategory_CategoryId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CategoryId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Sections");
        }
    }
}
