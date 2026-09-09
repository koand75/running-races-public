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
    public DbSet<RaceCategory> RaceCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Section>(entity =>
        {
            // StartWayPoint kapcsolat definiálása
            entity.HasOne(s => s.StartWayPoint)
                .WithMany() // Ha a WayPoint osztályban nincs ICollection<Section>
                .HasForeignKey(s => s.StartWayPointId)
                .OnDelete(DeleteBehavior.Restrict);

            // EndWayPoint kapcsolat definiálása
            entity.HasOne(s => s.EndWayPoint)
                .WithMany()
                .HasForeignKey(s => s.EndWayPointId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WayPoint>(entity =>
        {
            entity.HasOne(s => s.Race)
                .WithMany()
                .HasForeignKey(s => s.RaceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RunnerSection>(entity =>
        {
            entity.HasOne(s => s.Race)
                .WithMany()
                .HasForeignKey(s => s.RaceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RaceCategory>()
            .Property(rc => rc.RaceType)
            .HasConversion<string>();

        modelBuilder.Entity<RaceCategory>()
            .Property(rc => rc.Measurement)
            .HasConversion<string>();

        modelBuilder.Entity<Race>().HasData(
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Budapest Marathon",
                StartDate = new DateTime(2025, 10, 5),
                Location = "Budapest",
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Name = "Balaton Supermarathon",
                StartDate = new DateTime(2025, 7, 12),
                Location = "Balatonfüred",
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Name = "SPAR Budapest Half Marathon",
                StartDate = new DateTime(2025, 9, 14),
                Location = "Budapest",
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Race
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
                Name = "Telekom Vivicittá",
                StartDate = new DateTime(2025, 4, 6),
                Location = "Budapest",
                CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}