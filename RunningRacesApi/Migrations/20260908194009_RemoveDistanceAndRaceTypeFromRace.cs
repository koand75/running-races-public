using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDistanceAndRaceTypeFromRace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "RaceType",
                table: "Races");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "Races",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RaceType",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Date", "Distance", "RaceType" },
                values: new object[] { new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 42.200000000000003, "None" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                columns: new[] { "Date", "Distance", "RaceType" },
                values: new object[] { new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 195.0, "None" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                columns: new[] { "Date", "Distance", "RaceType" },
                values: new object[] { new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 21.100000000000001, "None" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                columns: new[] { "Date", "Distance", "RaceType" },
                values: new object[] { new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0, "None" });
        }
    }
}
