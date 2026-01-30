using SmartDayPlanner.Models;
using System.Text.Json;

namespace SmartDayPlanner.Clients
{
    public class OpenWeatherMapClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenWeatherMapClient> _logger;
        private readonly string _apiKey;

        public OpenWeatherMapClient(HttpClient httpClient, ILogger<OpenWeatherMapClient> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = config["OpenWeatherMap:ApiKey"] ?? throw new ArgumentNullException("OpenWeatherMap API key not set.");
        }

        public async Task<FetchedWeather> FetchWeatherConditionAsync(string city, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching weather for {City} from OpenWeatherMap API...", city);

            string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode(); 

            string json = await response.Content.ReadAsStringAsync(cancellationToken);

            var weatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(json);

            string condition = weatherResponse.Weather.FirstOrDefault()?.main.ToUpper() ?? "UNKNOWN";
            int temperature = (int)Math.Round(weatherResponse.Main.temp);

            _logger.LogDebug("Received actual weather: {Condition}, {Temp}°C", condition, temperature);

            return new FetchedWeather(condition, temperature);
        }
    }
}
