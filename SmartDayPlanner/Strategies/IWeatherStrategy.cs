using SmartDayPlanner.Models;

namespace SmartDayPlanner.Strategies
{
    public interface IWeatherStrategy
    {
        Task<List<Activity>> GetActivitiesAsync(UserPreferences preferences, CancellationToken cancellationToken);
    }
}
