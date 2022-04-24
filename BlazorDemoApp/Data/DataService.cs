/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace BlazorDemoApp.Data;

public class DataService
{
    public int CounterValue { get; set; }
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        var rng = new Random();
        
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray());
    }
}

// http://localhost:7777/WeatherForecast
