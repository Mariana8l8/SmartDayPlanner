using MongoDB.Bson; 
using SmartDayPlanner.Data;
using SmartDayPlanner.Delegates;
using SmartDayPlanner.Models;
using SmartDayPlanner.Strategies;

namespace SmartDayPlanner.Services
{
    public class DayPlanner
    {
        private readonly WeatherStation _weatherStation;
        private readonly ILogger<DayPlanner> _logger;
        private readonly MongoDbContext _dbContext;
        private readonly Dictionary<string, IWeatherStrategy> _strategies;

        public DayPlanner(
            WeatherStation weatherStation,
            ILogger<DayPlanner> logger,
            MongoDbContext mongoDbContext,
            IEnumerable<IWeatherStrategy> strategies)
        {
            _weatherStation = weatherStation;
            _logger = logger;
            _dbContext = mongoDbContext;

            _strategies = strategies.ToDictionary(
                strategy => strategy.GetType().Name.Replace("WeatherStrategy", "").ToUpper(),
                strategy => strategy
            );
        }

        public void SubscribeToWeatherUpdates()
        {
            _weatherStation.OnWeatherChange += HandleWeatherChange;
            _logger.LogInformation("DayPlanner is signed for weather updates");
        }


        private IWeatherStrategy GetStrategy(string condition)
        {
            string key = condition.ToUpper();
            if (_strategies.TryGetValue(key, out var strategy))
            {
                return strategy;
            }

            _logger.LogWarning("Strategy for {Condition} not found. Using CloudyWeatherStrategy.", condition);

            const string defaultKey = "CLOUDY";

            if (_strategies.ContainsKey(defaultKey))
            {
                return _strategies[defaultKey];
            }

            return _strategies.Values.First();
        }

        private async Task HandleWeatherChange(FetchedWeather newWeather)
        {
            var cancellationToken = CancellationToken.None;
            const string userId = "Mariana";

            _logger.LogInformation("[INFO] DayPlanner received a new weather alert: {Condition}", newWeather.Condition);

            try
            {
                UserPreferences preferences = await _dbContext.GetUserPreferencesAsync(userId, cancellationToken);

                IWeatherStrategy strategy = GetStrategy(newWeather.Condition);

                List<Activity> newActivities = await strategy.GetActivitiesAsync(preferences, cancellationToken);

                var newPlan = new Plan(
                    Id: ObjectId.Empty,
                    UserId: userId,
                    Date: DateTime.Today,
                    Location: "Lviv",
                    Weather: new WeatherInfo(newWeather.Condition, newWeather.Temperature),
                    Activities: newActivities
                );

                await _dbContext.SavePlanAsync(newPlan, cancellationToken);

                _logger.LogInformation("[INFO] The plan has been successfully updated and saved for {Condition}. Temperature: {Temp}°C. Activities: {Activities}",
                    newWeather.Condition, newWeather.Temperature, string.Join(", ", newActivities.Select(a => a.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling weather change: {Message}", ex.Message);
            }
        }
    }
}
