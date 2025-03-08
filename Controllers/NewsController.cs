using aaura.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace aaura.api.Controllers;
[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
    public readonly NewsService _newsService;
    public NewsController(NewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllNews()
    {
        try
        {
            var news = await _newsService.ProcessTodayNewsAsync();
            return Ok(news);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "server error " + ex.Message);
        }
    }

}
