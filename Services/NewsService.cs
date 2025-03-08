using System.Xml.Linq;
using aaura.api.Utils;
using Flurl.Http;

namespace aaura.api.Services;

public class NewsService
{
    public async Task<List<string>> ProcessTodayNewsAsync()
    {
        Console.WriteLine("Starting ProcessTodayNewsAsync...");
        var newsItems = new List<NewsItem>();

        foreach (var url in NewsUrls.All)
        {
            Console.WriteLine($"Fetching news from: {url}");
            var scrapedNews = await ScrapeNewsAsync(url);
            Console.WriteLine($"Scraped {scrapedNews.Count} news items from {url}.");
            newsItems.AddRange(scrapedNews);
        }

        var today = DateTime.UtcNow.Date;
        Console.WriteLine($"Filtering news for {today}");

        var finalNews = newsItems
            .Where(n => n.Date.Date == today)
            .Select(n => $"Title: {n.Title} - Description: {n.Description}")
            .ToList();

        Console.WriteLine($"Filtered {finalNews.Count} news items for today.");
        Console.WriteLine("ProcessTodayNewsAsync completed.");
        return finalNews;
    }

    private async Task<List<NewsItem>> ScrapeNewsAsync(string url)
    {
        try
        {
            var rssFeed = await url.GetStringAsync();
            var doc = XDocument.Parse(rssFeed);

            var items = doc.Descendants("item").Select(item => new NewsItem
            {
                Title = item.Element("title")?.Value ?? "ignore this item",
                Description = item.Element("description")?.Value ?? "ignore this item",
                Date = DateTime.Parse(item.Element("pubDate")?.Value)
            });

            Console.WriteLine($"Parsed {items.Count()} items from RSS feed.");
            return items.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scraping news from {url}: {ex.Message}");
            return new List<NewsItem>();
        }
    }
}