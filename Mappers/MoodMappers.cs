using aaura.api.Dtos;

namespace aaura.api.Mappers;
public static class MoodMappers
{
    public static MoodDto ToMoodDto(this Mood moodModel)
    {
        return new MoodDto
        {
            Date = moodModel.Date,
            Score = moodModel.Score,
            AverageScore = moodModel.Score,
        };
    }
}