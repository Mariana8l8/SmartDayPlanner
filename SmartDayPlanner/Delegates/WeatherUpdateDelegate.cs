using SmartDayPlanner.Models;

namespace SmartDayPlanner.Delegates
{
    public delegate Task WeatherUpdateHandler(FetchedWeather newWeather);
}
