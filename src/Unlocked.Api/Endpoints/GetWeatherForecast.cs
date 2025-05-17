using Microsoft.AspNetCore.Http.HttpResults;

namespace Unlocked.Api.Endpoints;

internal static class GetWeatherForecast
{
    internal static void MapGetWeatherForecast(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("weatherforecast", Handler)
            .WithName("GetWeatherForecast");
    }

    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy",
            "Hot", "Sweltering", "Scorching"
    ];

    internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    internal static Ok<WeatherForecastResponse[]> Handler()
    {
        var forecast = Enumerable.Range(1, 5)
            .Select(index =>
                new WeatherForecastResponse
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                )
            )
            .ToArray();

        return TypedResults.Ok(forecast);
    }
}
