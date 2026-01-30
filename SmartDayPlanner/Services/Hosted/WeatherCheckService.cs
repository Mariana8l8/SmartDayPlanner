using SmartDayPlanner.Clients;
using SmartDayPlanner.Models;

namespace SmartDayPlanner.Services.Hosted
{
    public class WeatherCheckService : BackgroundService
    {
        private readonly ILogger<WeatherCheckService> _logger;
        private readonly WeatherStation _weatherStation;
        private readonly OpenWeatherMapClient _weatherClient;

        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);

        public WeatherCheckService(
            ILogger<WeatherCheckService> logger,
            WeatherStation weatherStation,
            OpenWeatherMapClient weatherClient)
        {
            _logger = logger;
            _weatherStation = weatherStation;
            _weatherClient = weatherClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather Check Service Running...");

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("[INFO] Starting periodic weather check.");

                    const string city = "Lviv";

                    FetchedWeather weatherData = await _weatherClient.FetchWeatherConditionAsync(city, stoppingToken);

                    await _weatherStation.UpdateWeather(weatherData);

                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during periodic weather check.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
