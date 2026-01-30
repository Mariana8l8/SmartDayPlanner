# SmartDayPlanner

A smart, weather-aware day planning API built with **.NET 9** and **MongoDB**.  
The application automatically generates personalized daily plans based on real-time weather conditions and user preferences.

---

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

---

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

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development)
- OpenWeatherMap API Key ([Get one free](https://openweathermap.org/api))

---

## Configuration

### Clone the Repository

```bash
git clone https://github.com/Mariana8l8/SmartDayPlanner.git
cd SmartDayPlanner
```

### Set OpenWeatherMap API Key

Edit `SmartDayPlanner/appsettings.json`:

```json
{
  "OpenWeatherMap": {
    "ApiKey": "your_api_key_here"
  }
}
```

---

## Running with Docker (Recommended)

```bash
docker-compose up --build
```

The application will be available at:

- **API**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger

---

## Running Locally

### Start MongoDB

```bash
docker run -d -p 27017:27017 --name mongodb mongo:latest
```

### Update MongoDB Connection String

In `appsettings.json`:

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017"
  }
}
```

### Run the Application

```bash
dotnet run
```

---

## API Endpoints

### Plans

- `GET /api/plan/{city}` – Generate a plan based on current weather
- `GET /api/plan/history` – Get all saved plans
- `GET /api/plan/{city}/{date}` – Get plan for a specific city and date

### Preferences

- `GET /api/preferences` – Get user preferences
- `PUT /api/preferences` – Update user preferences

---

## Tech Stack

- **.NET 9.0** – Web API framework
- **C# 13.0** – Programming language
- **MongoDB** – NoSQL database
- **Serilog** – Structured logging
- **Swashbuckle** – Swagger/OpenAPI documentation
- **Docker & Docker Compose** – Containerization

---

## NuGet Packages

```xml
<PackageReference Include="MongoDB.Bson" Version="3.5.0" />
<PackageReference Include="MongoDB.Driver" Version="3.5.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="10.0.1" />
```

---

## Project Structure

```text
SmartDayPlanner/
├── Clients/
│   └── OpenWeatherMapClient.cs
├── Controllers/
│   ├── PlanController.cs
│   └── PreferencesController.cs
├── Core/
│   └── MongoSettings.cs
├── Data/
│   └── MongoDbContext.cs
├── Delegates/
│   └── WeatherUpdateDelegate.cs
├── Models/
│   ├── Activity.cs
│   ├── FetchedWeather.cs
│   ├── OpenWeatherResponse.cs
│   ├── Plan.cs
│   └── UserPreferences.cs
├── Services/
│   ├── DayPlanner.cs
│   ├── WeatherStation.cs
│   └── Hosted/
│       ├── DbInitializerService.cs
│       └── WeatherCheckService.cs
├── Strategies/
│   ├── IWeatherStrategy.cs
│   ├── SunnyWeatherStrategy.cs
│   ├── RainyWeatherStrategy.cs
│   ├── CloudyWeatherStrategy.cs
│   └── SnowyWeatherStrategy.cs
└── wwwroot/
```

---

## How It Works

1. `WeatherCheckService` runs in the background and checks weather every 30 seconds
2. `WeatherStation` notifies observers when weather conditions change
3. `DayPlanner` selects the appropriate weather strategy
4. The selected strategy generates activities based on user preferences
5. Generated plans are stored in MongoDB

---

## Development

### Build

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Logs

Logs are written to:

- Console output
- `logs/smartDayPlannerLogs.txt` (rolling daily)

---

## Docker Services

- **mongo** – MongoDB database (port 27017)
- **app** – SmartDayPlanner API (port 8080)

---

## Contributing

```text
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request
```

---

## License

This project is part of a design patterns laboratory exercise.

---

## Author

**Mariana8l8**

- GitHub: https://github.com/Mariana8l8
- Repository: https://github.com/Mariana8l8/SmartDayPlanner

---

## Support

For issues and questions, please open an issue on GitHub.
