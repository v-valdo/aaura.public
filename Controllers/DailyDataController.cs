using aaura.api.Services;
using Microsoft.AspNetCore.Mvc;
using aaura.api.Mappers;

namespace aaura.api.Controllers;

[ApiController]
[Route("api/dailydata")]
public class DailyDataController : ControllerBase
{
    private readonly ILogger<DailyDataController> _logger;
    private readonly IPromptService<DailyData> _dailyDataService;
    public DailyDataController(ILogger<DailyDataController> logger, DailyDataService dailyDataService)
    {
        _logger = logger;
        _dailyDataService = dailyDataService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetDailyData()
    {
        _logger.LogInformation($"Data Request from {HttpContext.Request.Path}");
        DailyData dailyData = await _dailyDataService.GetOrGenerateDailyData();

        if (dailyData == null || dailyData.Reasoning.Length == 0)
        {
            _logger.LogInformation("Some pieces of data are invalid");
            return NotFound();
        }

        return Ok(dailyData.ToDailyDataDto());
    }
}