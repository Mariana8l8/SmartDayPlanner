using SmartDayPlanner.Data;

namespace SmartDayPlanner.Services.Hosted
{
    public class DbInitializerService : IHostedService
    {
        private readonly ILogger<DbInitializerService> _logger;
        private readonly MongoDbContext _dbContext;
        private readonly DayPlanner _dayPlanner;

        private const int MaxRetries = 5;
        private readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(3);

        public DbInitializerService(
            ILogger<DbInitializerService> logger,
            MongoDbContext dbContext,
            DayPlanner dayPlanner)
        {
            _logger = logger;
            _dbContext = dbContext;
            _dayPlanner = dayPlanner; 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database initializer started. Waiting for MongoDB and performing setup...");


            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    await _dbContext.InitializeDefaultPreferencesAsync();

                    _logger.LogInformation("MongoDB connection successful and default preferences initialized.");

                    _dayPlanner.SubscribeToWeatherUpdates();
                    _logger.LogInformation("DayPlanner successfully subscribed to WeatherStation updates.");

                    return; 
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "MongoDB initialization failed (Attempt {Attempt}/{MaxRetries}). Retrying in {Delay} seconds...",
                        i + 1, MaxRetries, RetryDelay.TotalSeconds);

                    if (i == MaxRetries - 1)
                    {
                        _logger.LogError("FATAL: All MongoDB connection attempts failed. Application cannot proceed.");
                        throw;
                    }

                    await Task.Delay(RetryDelay, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database initializer stopped.");
            return Task.CompletedTask;
        }
    }
}
