using SmartDayPlanner.Models;

namespace SmartDayPlanner.Strategies
{
    public class SnowyWeatherStrategy : IWeatherStrategy
    {
        public Task<List<Activity>> GetActivitiesAsync(
            UserPreferences preferences,
            CancellationToken cancellationToken)
        {
            var activities = new List<Activity>();

            if (preferences.WorkingHours != null)
            {
                activities.Add(new Activity("Remote Conference Call", "productive", 1));
            }

            if ((preferences.PreferredTypes.Contains("outdoor") || 
                preferences.PreferredTypes.Contains("sport")) && 
                !preferences.AvoidTypes.Contains("sport"))
            {
                activities.Add(new Activity("Snow Activities", "sport", 2));
            }
            else
            {
                activities.Add(new Activity("Baking & Cooking", "indoor", 2));
            }

            if (!preferences.WeekendMode && !preferences.AvoidTypes.Contains("sport"))
            {
                activities.Add(new Activity("Home Workout", "sport", 3));
            }
            else
            {
                activities.Add(new Activity("Hot Chocolate & Netflix and Chill", "indoor", 3));
            }

            return Task.FromResult(activities.OrderBy(a => a.Priority).ToList());
        }
    }
}
