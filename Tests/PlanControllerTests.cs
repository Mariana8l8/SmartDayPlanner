using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartDayPlanner.Controllers;
using Xunit;
using Moq;
using SmartDayPlanner.Data;
using SmartDayPlanner.Models;
using SmartDayPlanner.Services;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

namespace Tests
{
    public class PlanControllerTests
    {
        public class PlanControllerTests
        {
            private readonly Mock<MongoDbContext> _mockDbContext;
            private readonly Mock<WeatherStation> _mockWeatherStation;
            private readonly PlanController _controller;

            // КОНСТРУКТОР ПОВИНЕН МАТИ ТІЛО ({}). Його відновлено.
            public PlanControllerTests()
            {
                _mockDbContext = TestMoqs.GetMongoDbContextMoq();
                _mockWeatherStation = TestMoqs.GetWeatherStationMoq();

                _controller = new PlanController(_mockDbContext.Object, _mockWeatherStation.Object);
            }

            [Fact]
            public async Task GetCurrentPlan_ReturnsOkWithCorrectData_WhenPlanExists()
            {
                // Arrange
                const string testUserId = "testUser";
                Plan expectedPlan = TestData.GetSamplePlan(testUserId);

                // Act
                var result = await _controller.GetCurrentPlan(testUserId);

                // Assert
                var okResult = result.Result as OkObjectResult;

                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                var actualPlan = okResult.Value as Plan;

                Assert.NotNull(actualPlan);
                Assert.Equal(testUserId, actualPlan.UserId);
                Assert.Equal("SUNNY", actualPlan.Weather.Condition);
                Assert.Equal(expectedPlan.Activities.Count, actualPlan.Activities.Count);
            }

            [Fact]
            public async Task GetCurrentPlan_ReturnsNotFound_WhenPlanDoesNotExist()
            {
                // Act
                var result = await _controller.GetCurrentPlan("unknownUser");

                // Assert
                var notFoundResult = result.Result as NotFoundObjectResult;
                Assert.NotNull(notFoundResult);
                Assert.Equal(404, notFoundResult.StatusCode);
            }

            [Fact]
            public async Task GetCurrentPlan_ReturnsNotFoundWithMessage_WhenNoPlanAndWeatherKnown()
            {
                // Arrange
                const string userId = "unknownUser";
                // WeatherStation має повертати "RAINY" з мока

                // Act
                var result = await _controller.GetCurrentPlan(userId);

                // Assert
                var notFoundResult = result.Result as NotFoundObjectResult;
                Assert.NotNull(notFoundResult);
                Assert.Contains(userId, notFoundResult.Value.ToString());
                Assert.Contains("RAINY", notFoundResult.Value.ToString());
            }

            [Fact]
            public async Task GetCurrentPlan_Returns500InternalServerError_OnDatabaseError()
            {
                // Act
                var result = await _controller.GetCurrentPlan("errorUser");

                // Assert
                var statusCodeResult = result.Result as ObjectResult;

                Assert.NotNull(statusCodeResult);
                Assert.Equal(500, statusCodeResult.StatusCode);
                Assert.Contains("Simulated DB connection failure", statusCodeResult.Value.ToString());
            }

            [Fact]
            public void GetCurrentWeather_ReturnsOkWithCondition()
            {
                // Arrange (Mock повертає "RAINY")

                // Act
                var result = _controller.GetCurrentWeather();

                // Assert
                var okResult = result.Result as OkObjectResult;
                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal("RAINY", okResult.Value);
            }
        }
}
