namespace aaura.api.Repositories;

public interface IDailyDataRepository
{
    public Task<List<DailyData>> GetAllDailyDataAsync();
    public Task<DailyData> AddDailyDataAsync(DailyData dailyData);
    public Task<DailyData> GetDailyDataByDateAsync(DateTime date);
    public Task<DailyData> RemoveDailyDataToday();
}
