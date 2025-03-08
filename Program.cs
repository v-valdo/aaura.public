using aaura.api.Data;
using aaura.api.Middleware;
using aaura.api.Repositories;
using aaura.api.Services;
using aaura.api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAI.Images;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.Configure<OpenAIOptions>(options =>
{
    builder.Configuration.GetSection("OpenAI").Bind(options);

    var apiKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
    if (!string.IsNullOrEmpty(apiKey))
    {
        options.ApiKey = apiKey;
    }
});

// Register services
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddScoped<IDailyDataRepository, DailyDataRepository>();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<IMoodScoreRepository, MoodScoreRepository>();
builder.Services.AddScoped<MoodScoreService>();
builder.Services.AddScoped<Extractor>();

builder.Services.AddSingleton<ChatClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    return new(options.GptModel, options.ApiKey);
});

builder.Services.AddSingleton<ImageClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    return new(options.DalleModel, options.ApiKey);
});

builder.Services.AddScoped<DailyDataService>();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// controllers
builder.Services.AddControllers();

// db
builder.Services.AddDbContext<aauraContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

var app = builder.Build();
app.UseCors("AllowFrontend");
// enable to block all requests w/o key
// app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
