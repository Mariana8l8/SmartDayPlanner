using MongoDB.Bson;

namespace SmartDayPlanner.Models
{
    public record Plan(
        ObjectId Id,
        string UserId,
        DateTime Date,
        string Location,
        WeatherInfo Weather,
        List<Activity> Activities
    );

    public record WeatherInfo(
        string Condition,
        int Temperature
    );
}
