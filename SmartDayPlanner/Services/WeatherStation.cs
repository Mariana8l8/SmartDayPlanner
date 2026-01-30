using SmartDayPlanner.Delegates;
using SmartDayPlanner.Models;

namespace SmartDayPlanner.Services
{
    public class WeatherStation
    {
        private readonly ILogger<WeatherStation> _logger;
        private FetchedWeather _currentWeather = new FetchedWeather("UNKNOWN", 0);

        public event WeatherUpdateHandler OnWeatherChange;

        public WeatherStation(ILogger<WeatherStation> logger)
        {
            _logger = logger;
        }

        public async Task UpdateWeather(FetchedWeather newWeather) 
        {
            string newCondition = newWeather.Condition.ToUpper(); 

            if (newWeather.Condition.ToUpper() != _currentWeather.Condition.ToUpper())
            {
                _logger.LogInformation("[INFO] Weather updated: {OldCondition} -> {NewCondition}",
                    _currentWeather.Condition, newWeather.Condition);

                _currentWeather = newWeather; 
                await NotifyObservers();
            }
            else
            {
                _logger.LogDebug("[DEBUG] Weather checked, no change: {Weather}", _currentWeather);
            }
        }

        private async Task NotifyObservers()
        {
            if (OnWeatherChange != null)
            {
                await OnWeatherChange.Invoke(_currentWeather);
            }
        }

        public string GetCurrentCondition() => _currentWeather.Condition;
    }
}
