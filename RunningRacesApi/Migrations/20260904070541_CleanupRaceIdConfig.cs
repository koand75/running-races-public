using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class CleanupRaceIdConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RaceId",
                table: "Sections",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "RaceId1",
                table: "Sections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Races",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RaceId1",
                table: "Sections",
                column: "RaceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Races_RaceId1",
                table: "Sections",
                column: "RaceId1",
                principalTable: "Races",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Races_RaceId1",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_RaceId1",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "RaceId1",
                table: "Sections");

            migrationBuilder.AlterColumn<Guid>(
                name: "RaceId",
                table: "Sections",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Races",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }
    }
}
