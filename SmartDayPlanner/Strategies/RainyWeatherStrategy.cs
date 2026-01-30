using SmartDayPlanner.Models;

namespace SmartDayPlanner.Strategies
{
    public class RainyWeatherStrategy : IWeatherStrategy
    {
        public Task<List<Activity>> GetActivitiesAsync(
            UserPreferences preferences,
            CancellationToken cancellationToken)
        {
            var activities = new List<Activity>();

            if (preferences.WorkingHours != null)
            {
                activities.Add(new Activity("Remote work", "productive", 1));
            }

            if (preferences.PreferredTypes.Contains("learning"))
            {
                activities.Add(new Activity("Studying", "productive", 2));
            }
            else
            {
                activities.Add(new Activity("House work", "indoor", 2));
            } 

            if (!preferences.WeekendMode)
            {
                activities.Add(new Activity("Read Book", "indoor", 3));
            }

            return Task.FromResult(activities.OrderBy(a => a.Priority).ToList());
        }
    }
}
