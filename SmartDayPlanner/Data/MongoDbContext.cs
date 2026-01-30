using MongoDB.Bson;
using MongoDB.Driver;
using SmartDayPlanner.Core;
using SmartDayPlanner.Models;

namespace SmartDayPlanner.Data
{
    public class MongoDbContext
    {
        private readonly IMongoCollection<Plan> _plans;
        private readonly IMongoCollection<UserPreferences> _preferences;
        public MongoDbContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _plans = database.GetCollection<Plan>(settings.PlansCollectionName);
            _preferences = database.GetCollection<UserPreferences>(settings.PreferencesCollectionName);

        }

        public async Task<UserPreferences> GetUserPreferencesAsync(string userId, CancellationToken cancellationToken)
        {
            var filter = Builders<UserPreferences>.Filter.Eq(p => p.UserId, userId);

            var prefs = await _preferences.Find(filter).FirstOrDefaultAsync(cancellationToken);

            return prefs ?? await GetUserPreferencesAsync("Mariana", cancellationToken);
        }

        public async Task<Plan?> GetLatestPlanAsync(string userId)
        {
            var filter = Builders<Plan>.Filter.Eq(p => p.UserId, userId);

            var latestPlan = await _plans.Find(filter)
                                         .SortByDescending(p => p.Date)
                                         .FirstOrDefaultAsync();
            return latestPlan;
        }

        public Task SavePlanAsync(Plan plan, CancellationToken cancellationToken)
        {
            return _plans.InsertOneAsync(plan, cancellationToken: cancellationToken);
        }

        public async Task InitializeDefaultPreferencesAsync()
        {
            if (!await _preferences.Find(p => p.UserId == "Mariana").AnyAsync())
            {
                var defaultPrefs = new UserPreferences(
                    ObjectId.GenerateNewId(),
                    "Mariana",
                    preferredTypes: new List<string> { "outdoor", "sport", "learning" },
                    avoidTypes: new List<string> { "date" },
                    workingHours: new WorkingHours(9, 17),
                    weekendMode: false
                );
                await _preferences.InsertOneAsync(defaultPrefs);
            }
        }

        public async Task SaveUserPreferencesAsync(UserPreferences preferences, CancellationToken cancellationToken)
        {

            var filter = Builders<UserPreferences>.Filter.Eq(p => p.UserId, preferences.UserId);

            var result = await _preferences.ReplaceOneAsync(
                filter,
                preferences,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken
            );
        }
    }
}
