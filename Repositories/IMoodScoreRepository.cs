using aaura.api.Models;

public interface IMoodScoreRepository
{
    Task<List<Mood>> GetMoodScoresAsync();
    Task<Mood> AddMoodScoreAsync(int score, int roundedAverage, string avgStatus);
    Task<bool> CheckMoodScoreExistsAsync(DateTime date);
    Task<Mood> GetLatestMoodScoreAsync();
    Task<ScoreSumDto> GetLastTenScoresCountAndSumAsync();
}