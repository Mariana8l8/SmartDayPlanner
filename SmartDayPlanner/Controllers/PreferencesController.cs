using Microsoft.AspNetCore.Mvc;
using SmartDayPlanner.Data;
using SmartDayPlanner.Models;

namespace SmartDayPlanner.Controllers
{
    [ApiController]
    [Route("api/preferences")]
    public class PreferencesController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public PreferencesController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePreferences([FromBody] UserPreferences preferences)
        {
            if (string.IsNullOrEmpty(preferences.UserId))
            {
                return BadRequest("User ID must be provided.");
            }

            await _dbContext.SaveUserPreferencesAsync(preferences, CancellationToken.None);

            return Ok($"User settings {preferences.UserId} saved successfully.");
        }
    }
}