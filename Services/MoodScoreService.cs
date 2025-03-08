namespace aaura.api.Services;

public class MoodScoreService
{
    private readonly IMoodScoreRepository _repository;
    public MoodScoreService(IMoodScoreRepository repository)
    {
        _repository = repository;
    }
    public async Task<Mood> AddMoodScoreAsync(int score)
    {
        var scoreSummary = await _repository.GetLastTenScoresCountAndSumAsync();

        int roundedAverage = CalculateNewAverage(score, scoreSummary.ScoreCount, scoreSummary.ScoreSum);

        var latestMood = await _repository.GetLatestMoodScoreAsync();
        string avgStatus = DetermineAverageStatus(latestMood?.AverageScore, roundedAverage);

        return await _repository.AddMoodScoreAsync(score, roundedAverage, avgStatus);
    }
    public async Task<IEnumerable<Mood>> GetAllMoodScores()
    {
        return await _repository.GetMoodScoresAsync();
    }
    public async Task<bool> CheckMoodScoreExists()
    {
        var todaysDate = DateTime.UtcNow.Date;
        return await _repository.CheckMoodScoreExistsAsync(todaysDate);
    }

    public async Task<Mood> GetLatestMoodScoreAsync()
    {
        return await _repository.GetLatestMoodScoreAsync();
    }

    private int CalculateNewAverage(int score, int scoreCount, int scoreSum)
    {
        if (score == 0)
        {
            return (int)Math.Round(scoreSum / (double)scoreCount, MidpointRounding.AwayFromZero);
        }

        double newAverage = score == 0
            ? scoreSum / (double)(scoreCount + 1)
            : (scoreSum + score) / (double)(scoreCount + 1);

        return (int)Math.Round(newAverage, MidpointRounding.AwayFromZero);
    }

    private string DetermineAverageStatus(int? latestAverageScore, int newAverage)
    {
        if (!latestAverageScore.HasValue) return "Same";

        return newAverage > latestAverageScore ? "Up"
             : newAverage < latestAverageScore ? "Down"
             : "Same";
    }
}
