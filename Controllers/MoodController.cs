using aaura.api.Mappers;
using aaura.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace aaura.api.Controllers;
[ApiController]
[Route("api/mood")]
public class MoodController : ControllerBase
{
    private readonly MoodScoreService _service;
    public MoodController(MoodScoreService service)
    {
        _service = service;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllMoodScores()
    {
        var moodScores = await _service.GetAllMoodScores();
        var moodScoresDto = moodScores.Select(m => m.ToMoodDto()).ToList();

        if (moodScores == null || !moodScores.Any())
        {
            return NoContent();
        }
        return Ok(moodScoresDto);
    }
}