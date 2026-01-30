using Microsoft.Extensions.Options;
using Serilog;
using SmartDayPlanner.Clients;
using SmartDayPlanner.Core;
using SmartDayPlanner.Data;
using SmartDayPlanner.Services;
using SmartDayPlanner.Services.Hosted;
using SmartDayPlanner.Strategies;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
   .WriteTo.Console()
   .WriteTo.File("log/smartDayPlannerLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection(nameof(MongoSettings)));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MongoSettings>>().Value);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddSingleton<OpenWeatherMapClient>();

builder.Services.AddSingleton<WeatherStation>();

builder.Services.AddSingleton<IWeatherStrategy, SunnyWeatherStrategy>();
builder.Services.AddSingleton<IWeatherStrategy, RainyWeatherStrategy>();
builder.Services.AddSingleton<IWeatherStrategy, CloudyWeatherStrategy>();
builder.Services.AddSingleton<IWeatherStrategy, SnowyWeatherStrategy>();

builder.Services.AddSingleton<DayPlanner>();

builder.Services.AddHostedService<WeatherCheckService>();
builder.Services.AddHostedService<DbInitializerService>();

builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<OpenWeatherMapClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();