using aaura.api.Dtos;

namespace aaura.api.Mappers;

public static class DailyDataMappers
{
    public static DailyDataDto ToDailyDataDto(this DailyData dailyDataModel)
    {
        return new DailyDataDto
        {
            Date = dailyDataModel.Date,
            Score = dailyDataModel.Mood.Score,
            AverageScore = dailyDataModel.Mood.AverageScore,
            AvgStatus = dailyDataModel.Mood.AvgStatus,
            Reasoning = dailyDataModel.Reasoning,
            ImgUrl = dailyDataModel.ImgUrl
        };
    }
}