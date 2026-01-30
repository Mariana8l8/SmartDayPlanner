using System.Text.Json.Serialization;

namespace SmartDayPlanner.Models
{
    public record OpenWeatherResponse(
        [property: JsonPropertyName("main")] MainInfo Main,
        [property: JsonPropertyName("weather")] List<ApiWeatherInfo> Weather
    );

    public record MainInfo(double temp); 
    public record ApiWeatherInfo(string main); 
}
