using System.ComponentModel.DataAnnotations;

public class Mood
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Date { get; set; }

    [Required]
    public int Score { get; set; }
    public int? AverageScore { get; set; }
    public string? AvgStatus { get; set; }
}