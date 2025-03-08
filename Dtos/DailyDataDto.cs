namespace aaura.api.Dtos;
public class DailyDataDto
{
    public string Date { get; set; }
    public int Score { get; set; }
    public int? AverageScore { get; set; }
    public string? Reasoning { get; set; }
    public string? ImgUrl { get; set; }
    public string? AvgStatus { get; set; }
}