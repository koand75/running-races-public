using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunningRacesApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRaceCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE Races SET RaceType = CASE RaceType 
                 WHEN '0' THEN 'None'
                 WHEN '1' THEN 'Relay'
                 ELSE 'None'
               END;");
            migrationBuilder.Sql(@"
               INSERT INTO RaceCategory (RaceId, Name, RaceType, Measurement, Distance)
               SELECT Id, Name, 
               CASE WHEN RaceType = 'Relay' THEN 'Relay' ELSE 'None' END,
               'None', Distance FROM Races
           ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
