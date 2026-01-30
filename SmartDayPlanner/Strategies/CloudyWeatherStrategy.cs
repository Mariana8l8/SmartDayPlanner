using SmartDayPlanner.Models;

namespace SmartDayPlanner.Strategies
{
    public class CloudyWeatherStrategy : IWeatherStrategy
    {
        public Task<List<Activity>> GetActivitiesAsync(
            UserPreferences preferences,
            CancellationToken cancellationToken)
        {
            var activities = new List<Activity>();

            if (preferences.WorkingHours != null)
            {
                activities.Add(new Activity("Focus Work", "productive", 1));
            }

            if (preferences.PreferredTypes.Contains("learning") && !preferences.AvoidTypes.Contains("learning"))
            {
                activities.Add(new Activity("Visit Museum", "learning", 2));
            }
            else if (!preferences.AvoidTypes.Contains("outdoor"))
            {
                activities.Add(new Activity("City Exploration", "outdoor", 2));
            }

            if (preferences.WeekendMode && !preferences.AvoidTypes.Contains("date"))
            {
                activities.Add(new Activity("Cozy Dinner Date", "date", 3));
            }
            else
            {
                activities.Add(new Activity("Indoor Reading", "indoor", 3));
            }

            return Task.FromResult(activities.OrderBy(a => a.Priority).ToList());
        }
    }
}
