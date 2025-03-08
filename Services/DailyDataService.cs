using aaura.api.Repositories;
using aaura.api.Utils;
using OpenAI.Chat;
using OpenAI.Images;

namespace aaura.api.Services;

public class DailyDataService : IPromptService<DailyData>
{
    private readonly ChatClient _chatClient;
    private readonly ImageClient _imageClient;
    private readonly NewsService _newsService;
    private readonly Extractor _extractor;
    private readonly MoodScoreService _moodScoreService;
    private readonly IDailyDataRepository _repository;
    private readonly CacheService _cacheService;
    public DailyDataService(
       NewsService newsService,
       MoodScoreService moodScoreService,
       IDailyDataRepository repository,
       ChatClient chatClient,
       ImageClient imageClient,
       Extractor extractor,
       CacheService cacheService)
    {
        _newsService = newsService;
        _moodScoreService = moodScoreService;
        _repository = repository;
        _chatClient = chatClient;
        _imageClient = imageClient;
        _extractor = extractor;
        _cacheService = cacheService;
    }

    public async Task<DailyData> GetOrGenerateDailyData()
    {
        var today = DateTime.UtcNow.ToShortDateString();

        var cachedData = await _cacheService.GetOrSetAsync(
            "dailyDataKey",
            async () =>
            {
                var dbData = await _repository.GetDailyDataByDateAsync(DateTime.UtcNow);
                if (dbData != null && dbData.Date == today)
                {
                    Console.WriteLine("Daily data found in DB for today");
                    return dbData;
                }

                Console.WriteLine("No data in DB today. Generating new.");
                return await GenerateNewDailyData();
            },
            TimeSpan.FromMinutes(30)
        );

        return cachedData;
    }

    public async Task<DailyData> GenerateNewDailyData()
    {
        var todaysNews = await _newsService.ProcessTodayNewsAsync();

        if (todaysNews == null || !todaysNews.Any())
        {
            return new();
        }

        Console.WriteLine($"=== Prompting Open AI with {todaysNews.Count()} Headlines ===");
        string prompt = Prompts.DailyPrompt(string.Join("\n", todaysNews));
        ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);
        string response = completion.Content[0].Text;

        Console.WriteLine($"=== Prompt Response Received w/ Char Length: {response.Length} ===");
        Console.WriteLine(response);

        Console.WriteLine($"=== Starting extraction ===");
        int score = _extractor.ExtractMoodScore(response);
        string reasoning = _extractor.ExtractReasoning(response);
        Console.WriteLine($@"
        === Extraction Completed: 
        Score - {score}
        ===");

        // System.Console.WriteLine("skipping image");
        Console.WriteLine($"=== Running Image Prompt ===");
        string imgUrl = await GenerateDailyImageUrl(reasoning);
        Console.WriteLine($"=== Image Url Received: {imgUrl} ===");
        //comment out to avoid img costs

        Console.WriteLine($"=== Adding Mood Score to Database... ===");
        Mood mood = await _moodScoreService.AddMoodScoreAsync(score);

        DailyData dailySummary = new()
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Reasoning = reasoning,
            ImgUrl = imgUrl,
            MoodId = mood.Id
        };

        await _repository.AddDailyDataAsync(dailySummary);

        Console.WriteLine($"=== Data Added To Db===");

        return dailySummary;
    }

    private async Task<string> GenerateDailyImageUrl(string reasoning)
    {
        ImageGenerationOptions options = new()
        {
            Quality = GeneratedImageQuality.Standard,
            Size = GeneratedImageSize.W1024xH1024,
            Style = GeneratedImageStyle.Vivid,
            ResponseFormat = GeneratedImageFormat.Uri
        };
        string prompt = Prompts.ImagePrompt(reasoning);
        GeneratedImage image = await _imageClient.GenerateImageAsync(prompt, options);
        return image.ImageUri.ToString();
    }
}