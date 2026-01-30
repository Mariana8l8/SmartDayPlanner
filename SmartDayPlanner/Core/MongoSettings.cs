namespace SmartDayPlanner.Core
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string PlansCollectionName { get; set; } = "plans";
        public string PreferencesCollectionName { get; set; } = "preferences";
    }
}
