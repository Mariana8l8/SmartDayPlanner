using SmartDayPlanner.Models;

namespace SmartDayPlanner.Strategies
{
    public class SunnyWeatherStrategy : IWeatherStrategy
    {
        public Task<List<Activity>> GetActivitiesAsync(
            UserPreferences preferences, 
            CancellationToken cancellationToken)
        {
            var activities = new List<Activity>();

            if (preferences.WorkingHours != null)
            {
                activities.Add(new Activity("Working", "productive", 1));
            }

            if (!preferences.AvoidTypes.Contains("outdoor"))
            {
                if (!preferences.AvoidTypes.Contains("hiking"))
                {
                    activities.Add(new Activity("Hiking", "outdoor", 2));
                }
                else
                {
                    activities.Add(new Activity("Park Walk", "outdoor", 2));
                }
            }

            if (preferences.PreferredTypes.Contains("sport") && !preferences.AvoidTypes.Contains("sport"))
            {
                activities.Add(new Activity("Cycling", "sport", 3));
            }

            return Task.FromResult(activities.OrderBy(a => a.Priority).ToList());
        }
    }
}
