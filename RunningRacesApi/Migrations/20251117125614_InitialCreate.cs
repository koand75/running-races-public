using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Distance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Races",
                columns: new[] { "Id", "Date", "Distance", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 42.200000000000003, "Budapest", "Budapest Marathon" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 195.0, "Balatonfüred", "Balaton Supermarathon" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 21.100000000000001, "Budapest", "SPAR Budapest Half Marathon" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0, "Budapest", "Telekom Vivicittá" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Races");
        }
    }
}
