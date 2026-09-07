using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations;

/// <inheritdoc />
public partial class FixSeedDates : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "Date",
            value: new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "Date",
            value: new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "Date",
            value: new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "Date",
            value: new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "Date",
            value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "Date",
            value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "Date",
            value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Races",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "Date",
            value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }
}
