using Microsoft.AspNetCore.Mvc;
using SmartDayPlanner.Data;
using SmartDayPlanner.Models;
using SmartDayPlanner.Services;

namespace SmartDayPlanner.Controllers
{
    [ApiController]
    [Route("api/plan")]
    public class PlanController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;
        private readonly WeatherStation _weatherStation;

        public PlanController(MongoDbContext dbContext, WeatherStation weatherStation)
        {
            _dbContext = dbContext;
            _weatherStation = weatherStation;
        }

        [HttpGet("current/{userId}")]
        public async Task<ActionResult<Plan>> GetCurrentPlan(string userId)
        {
            var latestPlan = await _dbContext.GetLatestPlanAsync(userId);

            if (latestPlan == null)
            {
                return NotFound($"User plan {userId} has not been generated yet. Current weather: {_weatherStation.GetCurrentCondition()}");
            }

            return Ok(latestPlan);
        }

        [HttpGet("weather")]
        public ActionResult<string> GetCurrentWeather()
        {
            return Ok(_weatherStation.GetCurrentCondition());
        }
    }
}
