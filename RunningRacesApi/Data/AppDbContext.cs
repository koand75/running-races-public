using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Models;

namespace RunningRacesApi.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Race> Races { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Runner> Runners { get; set; }
    public DbSet<RunnerSection> RunnerSections { get; set; }
    public DbSet<WayPoint> WayPoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Section>(entity =>
        {
            // StartWayPoint kapcsolat definiálása
            entity.HasOne(s => s.StartWayPoint)
                .WithMany() // Ha a WayPoint osztályban nincs ICollection<Section>
                .HasForeignKey(s => s.StartWayPointId)
                .OnDelete(DeleteBehavior.Restrict); // Fontos a Restrict, hogy ne legyen körkörös törlés

            // EndWayPoint kapcsolat definiálása
            entity.HasOne(s => s.EndWayPoint)
                .WithMany()
                .HasForeignKey(s => s.EndWayPointId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Race>().HasData(
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Budapest Marathon",
                Date = new DateTime(2025, 10, 5),
                Location = "Budapest",
                Distance = 42.2,
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Name = "Balaton Supermarathon",
                Date = new DateTime(2025, 7, 12),
                Location = "Balatonfüred",
                Distance = 195,
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Name = "SPAR Budapest Half Marathon",
                Date = new DateTime(2025, 9, 14),
                Location = "Budapest",
                Distance = 21.1,
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
                Name = "Telekom Vivicittá",
                Date = new DateTime(2025, 4, 6),
                Location = "Budapest",
                Distance = 10,
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}