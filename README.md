# SmartDayPlanner

A smart, weather-aware day planning API built with **.NET 9** and **MongoDB**. The application automatically generates personalized daily plans based on real-time weather conditions and user preferences.

## Features

- **Real-time Weather Integration**: Fetches live weather data from OpenWeatherMap API
- **Strategy Pattern**: Dynamically selects activity recommendations based on weather conditions (sunny, rainy, cloudy, snowy)
- **Observer Pattern**: Weather station notifies day planner when conditions change
- **User Preferences**: Customizable user settings for personalized recommendations
- **Automated Weather Updates**: Background service checks weather periodically
- **MongoDB Integration**: Persistent storage for plans and user preferences
- **Docker Support**: Fully containerized with Docker Compose
- **Swagger UI**: Interactive API documentation
- **Structured Logging**: Serilog integration with file and console output

## Architecture & Design Patterns

### Design Patterns Implemented

1. **Strategy Pattern** (`IWeatherStrategy`)
   - `SunnyWeatherStrategy`
   - `RainyWeatherStrategy`
   - `CloudyWeatherStrategy`
   - `SnowyWeatherStrategy`

2. **Observer Pattern**
   - `WeatherStation` (Subject)
   - `DayPlanner` (Observer)
   - `WeatherUpdateDelegate` (Event handler)

3. **Repository Pattern**
   - `MongoDbContext` for data access

4. **Dependency Injection**
   - Services registered in `Program.cs`

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development)
- OpenWeatherMap API Key ([Get one free](https://openweathermap.org/api))

### Configuration

1. **Clone the repository**

   cd SmartDayPlanner
   git clone https://github.com/Mariana8l8/SmartDayPlanner.git
   
3. **Set your OpenWeatherMap API Key**
   
   Edit `SmartDayPlanner/appsettings.json`:
   { "OpenWeatherMap": { "ApiKey": "your_api_key_here" } }

### Running with Docker (Recommended)

    docker-compose up --build

The application will be available at:
- **API**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger

### Running Locally

1. **Start MongoDB** (use Docker or local installation)

    docker run -d -p 27017:27017 --name mongodb mongo:latest

2. **Update connection string** in `appsettings.json`:

    "MongoSettings": { "ConnectionString": "mongodb://localhost:27017" }

3. **Run the application**

    dotnet run
    cd SmartDayPlanner

## API Endpoints

### Plans

- `GET /api/plan/{city}` - Generate a plan based on current weather
- `GET /api/plan/history` - Get all saved plans
- `GET /api/plan/{city}/{date}` - Get plan for specific city and date

### Preferences

- `GET /api/preferences` - Get user preferences
- `PUT /api/preferences` - Update user preferences

## Tech Stack

- **.NET 9.0** - Web API framework
- **C# 13.0** - Programming language
- **MongoDB** - NoSQL database
- **Serilog** - Structured logging
- **Swashbuckle** - Swagger/OpenAPI documentation
- **Docker & Docker Compose** - Containerization

## NuGet Packages

    <PackageReference Include="MongoDB.Bson" Version="3.5.0" /> 
    <PackageReference Include="MongoDB.Driver" Version="3.5.0" /> 
    <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" /> 
    <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" /> 
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.0.1" />

## Project Structure

SmartDayPlanner/
├── Clients/
│   └── OpenWeatherMapClient.cs        // External API integration
│
├── Controllers/
│   ├── PlanController.cs              // Plan endpoints
│   └── PreferencesController.cs       // Preferences endpoints
│
├── Core/
│   └── MongoSettings.cs               // MongoDB configuration
│
├── Data/
│   └── MongoDbContext.cs              // Database context
│
├── Delegates/
│   └── WeatherUpdateDelegate.cs       // Event delegate
│
├── Models/
│   ├── Activity.cs
│   ├── FetchedWeather.cs
│   ├── OpenWeatherResponse.cs
│   ├── Plan.cs
│   └── UserPreferences.cs
│
├── Services/
│   ├── DayPlanner.cs                  // Core planning logic
│   ├── WeatherStation.cs              // Weather observer subject
│   └── Hosted/
│       ├── DbInitializerService.cs    // Database initialization
│       └── WeatherCheckService.cs     // Background weather updates
│
├── Strategies/
│   ├── IWeatherStrategy.cs
│   ├── SunnyWeatherStrategy.cs
│   ├── RainyWeatherStrategy.cs
│   ├── CloudyWeatherStrategy.cs
│   └── SnowyWeatherStrategy.cs
│
└── wwwroot/

## How It Works

1. **Weather Monitoring**: `WeatherCheckService` runs in the background, checking weather every 30 seconds
2. **Weather Updates**: When weather changes, `WeatherStation` notifies subscribed observers
3. **Strategy Selection**: `DayPlanner` selects appropriate strategy based on weather condition
4. **Plan Generation**: Selected strategy generates activities based on user preferences
5. **Persistence**: Plans are saved to MongoDB for history tracking

## Development

### Building

    dotnet build

### Running Tests

    dotnet test

### Viewing Logs

Logs are written to:
- Console output
- `log/smartDayPlannerLogs.txt` (rolling daily)

## Docker Services

- **mongo**: MongoDB database (port 27017)
- **app**: SmartDayPlanner API (port 8080)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is part of a design patterns laboratory exercise.

## Author

**Mariana8l8**

- GitHub: [@Mariana8l8](https://github.com/Mariana8l8)
- Repository: [SmartDayPlanner](https://github.com/Mariana8l8/SmartDayPlanner)

## Support

For issues and questions, please open an issue on GitHub.
