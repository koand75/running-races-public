using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Repositories;

/// <summary>
/// Futóversenyek repository implementáció
/// </summary>
public class RaceRepository : IRaceRepository
{
    private readonly AppDbContext _context;

    public RaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Race>> GetRacesAsync(RaceSearchModel searchModel)
    {
        IQueryable<Race> query = _context.Races;

        if (searchModel.IsActive.HasValue)
        {      
            query = query.Where(r => r.IsActive == searchModel.IsActive.Value);
        }

        if (!string.IsNullOrEmpty(searchModel.SearchTerm))
        {
            var searchTerm = searchModel.SearchTerm.ToLower();
            var searchField = searchModel.SearchField?.ToLower() ?? "all";

            query = searchField switch
            {
                "name" => query.Where(r => r.Name.ToLower().Contains(searchTerm)),
                "location" => query.Where(r => r.Location.ToLower().Contains(searchTerm)),
                "all" => query.Where(r =>
                    r.Name.ToLower().Contains(searchTerm) ||
                    r.Location.ToLower().Contains(searchTerm)),
                _ => query
            };
        }

        var totalCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(searchModel.SortBy))
        {
            var sortBy = searchModel.SortBy.ToLower();
            var sortDirection = searchModel.SortDirection?.ToLower() ?? "asc";

            query = sortBy switch
            {
                "name" => sortDirection == "asc"
                    ? query.OrderBy(r => r.Name)
                    : query.OrderByDescending(r => r.Name),
                "date" => sortDirection == "asc"
                    ? query.OrderBy(r => r.Date)
                    : query.OrderByDescending(r => r.Date),
                "location" => sortDirection == "asc"
                    ? query.OrderBy(r => r.Location)
                    : query.OrderByDescending(r => r.Location),
                "distance" => sortDirection == "asc"
                    ? query.OrderBy(r => r.Distance)
                    : query.OrderByDescending(r => r.Distance),
                _ => query
            };
        }

        var items = await query
            .Skip((searchModel.Page - 1) * searchModel.PageSize)
            .Take(searchModel.PageSize)
            .ToListAsync();

        return new PagedResult<Race>
        {
            Items = items,
            TotalCount = totalCount,
            Page = searchModel.Page,
            PageSize = searchModel.PageSize
        };
    }

    public async Task<Race?> GetByIdAsync(Guid id)
    {
        return await _context.Races.FindAsync(id);
    }

    public async Task<Race> CreateAsync(Race race)
    {
        // Validáció
        if (string.IsNullOrWhiteSpace(race.Name))
        {
            throw new ArgumentException("Race name cannot be empty.", nameof(race.Name));
        }

        if (race.Distance <= 0)
        {
            throw new ArgumentException("Distance must be positive.", nameof(race.Distance));
        }

        _context.Races.Add(race);
        await _context.SaveChangesAsync();
        return race;
    }

    public async Task<Race> UpdateAsync(Guid id, Race race)
    {
        var existingRace = await _context.Races.FindAsync(id);
        if (existingRace == null )
        {
            throw new InvalidOperationException("Race not found");
        }

        existingRace.Name = race.Name;
        existingRace.Date = race.Date;
        existingRace.Location = race.Location;
        existingRace.Distance = race.Distance;
        existingRace.ModifiedAt = DateTime.UtcNow;
        existingRace.IsActive = race.IsActive;

        await _context.SaveChangesAsync();
        return existingRace;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var race = await _context.Races.FindAsync(id);
        if (race == null)
        {
            return false;
        }

        race.IsActive = false;
        race.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}