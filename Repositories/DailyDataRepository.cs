using aaura.api.Data;
using Microsoft.EntityFrameworkCore;

namespace aaura.api.Repositories;
public class DailyDataRepository : IDailyDataRepository
{
    private readonly aauraContext _context;
    public DailyDataRepository(aauraContext context)
    {
        _context = context;
    }
    public async Task<List<DailyData>> GetAllDailyDataAsync()
    {
        return await _context.DailyDatas
            .Include(d => d.Mood)
            .ToListAsync();
    }
    public async Task<DailyData> AddDailyDataAsync(DailyData dailyData)
    {
        _context.DailyDatas.Add(dailyData);
        await _context.SaveChangesAsync();
        return dailyData;
    }

    public async Task<DailyData> GetDailyDataByDateAsync(DateTime date)
    {
        return await _context.DailyDatas
            .Include(d => d.Mood)
            .Where(d => d.Date == date.ToShortDateString())
            .FirstOrDefaultAsync() ?? new();
    }

    public async Task<DailyData> RemoveDailyDataToday()
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var dailyData = await _context.DailyDatas.FirstOrDefaultAsync(d => d.Date == today);
        if (dailyData != null)
        {
            _context.Remove(dailyData);
            await _context.SaveChangesAsync();
        }
        return null;
    }
}