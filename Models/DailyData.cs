using System.ComponentModel.DataAnnotations;

public class DailyData
{
    [Required]
    public int Id { get; set; }
    public string? Reasoning { get; set; }

    [Required]
    public string Date { get; set; }

    public string? ImgUrl { get; set; }

    [Required]
    public int MoodId { get; set; }

    [Required]
    public Mood Mood { get; set; }
}