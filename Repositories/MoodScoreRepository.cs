using System.Collections.Immutable;
using aaura.api.Data;
using aaura.api.Models;
using Microsoft.EntityFrameworkCore;

public class MoodScoreRepository : IMoodScoreRepository
{
    private readonly aauraContext _context;
    public MoodScoreRepository(aauraContext context)
    {
        _context = context;
    }

    public async Task<List<Mood>> GetMoodScoresAsync()
    {
        return await _context.Moods.ToListAsync();
    }

    public async Task<Mood> GetLatestMoodScoreAsync()
    {
        return await _context.Moods.OrderByDescending(m => m.Date).FirstOrDefaultAsync();
    }

    public async Task<ScoreSumDto> GetLastTenScoresCountAndSumAsync()
    {
        var lastTenMoods = await _context.Moods
            .OrderByDescending(m => m.Date)
            .Take(10)
            .ToListAsync();

        var scoreCount = lastTenMoods.Count(m => m.Score > 0);
        var scoreSum = lastTenMoods.Where(m => m.Score > 0).Sum(m => m.Score);

        return new ScoreSumDto
        {
            ScoreCount = scoreCount,
            ScoreSum = scoreSum
        };
    }
    public async Task<Mood> AddMoodScoreAsync(int score, int roundedAverage, string avgStatus)
    {
        Mood mood = new()
        {
            Score = score,
            Date = DateTime.UtcNow.ToShortDateString(),
            AverageScore = roundedAverage,
            AvgStatus = avgStatus,
        };

        _context.Moods.Add(mood);
        await _context.SaveChangesAsync();
        return mood;
    }

    public async Task<bool> CheckMoodScoreExistsAsync(DateTime date)
    {
        return await _context.Moods.AnyAsync(m => m.Date == date.ToShortDateString());
    }
}