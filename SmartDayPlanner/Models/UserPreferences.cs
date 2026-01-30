using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartDayPlanner.Models
{
    public record UserPreferences
    {
        [BsonId]
        public ObjectId Id { get; init; } 
        public string UserId { get; init; }
        public List<string> PreferredTypes { get; init; }
        public List<string> AvoidTypes { get; init; }
        public WorkingHours? WorkingHours { get; init; }
        public bool WeekendMode { get; init; }

        public UserPreferences(
            ObjectId id,
            string userId,
            List<string> preferredTypes,
            List<string> avoidTypes,
            WorkingHours? workingHours,
            bool weekendMode)
        {
            Id = id;
            UserId = userId;
            PreferredTypes = preferredTypes;
            AvoidTypes = avoidTypes;
            WorkingHours = workingHours;
            WeekendMode = weekendMode;
        }
        public UserPreferences() { }
    }

    public record WorkingHours(int Start, int End);
}