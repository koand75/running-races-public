using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Repositories;

public class WayPointRepository : IWayPointRepository
{
    private readonly AppDbContext _context;

    public WayPointRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<WayPoint>> GetAllAsync(BaseSearchModel? searchModel)
    {
        if (searchModel is null)
        {
            searchModel = new BaseSearchModel();
        }

        IQueryable<WayPoint> query = _context.WayPoints;

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((searchModel.Page - 1) * searchModel.PageSize)
            .Take(searchModel.PageSize)
            .ToListAsync();

        return new PagedResult<WayPoint>
        {
            Items = items,
            TotalCount = totalCount,
            Page = searchModel.Page,
            PageSize = searchModel.PageSize
        };
    }

    public async Task<WayPoint> CreateAsync(WayPoint wayPoint)
    {
        _context.WayPoints.Add(wayPoint);
        await _context.SaveChangesAsync();

        return wayPoint;
    }

    public async Task<WayPoint?> GetByIdAsync(int? id)
    {
        return await _context.WayPoints.FindAsync(id);
    }
    public async Task<WayPoint?> UpdateAsync(int id, WayPoint wayPoint)
    {
        var existing = await _context.WayPoints.FindAsync(id);

        if (existing == null) return null;

        existing.Name = wayPoint.Name;
        existing.Lat = wayPoint.Lat;
        existing.Lng = wayPoint.Lng;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usedWayPoint = await _context.Sections
            .AnyAsync(s => s.StartWayPointId == id || s.EndWayPointId == id);

        if (usedWayPoint) return false;

        var wp = await _context.WayPoints.FindAsync(id);

        if (wp == null) return false;

        _context.WayPoints.Remove(wp);
        await _context.SaveChangesAsync();

        return true;
    }
}