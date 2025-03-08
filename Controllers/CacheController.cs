using aaura.api.Repositories;
using aaura.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace aaura.api.Controllers;
[ApiController]
[Route("api/cache")]
public class CacheController : ControllerBase
{
    private readonly CacheService _cacheService;
    private readonly IDailyDataRepository _repository;
    public CacheController(CacheService cacheService, IDailyDataRepository repository)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> ClearCacheKey(string key)
    {
        await _cacheService.ClearAsync(key);
        return NoContent();
    }

    [HttpDelete("db")]
    public async Task<IActionResult> ClearDb()
    {
        var removedData = await _repository.RemoveDailyDataToday();
        if (removedData == null)
        {
            return NotFound(new { message = "Data for today not found." });
        }
        return NoContent();
    }
}