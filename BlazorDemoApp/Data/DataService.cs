/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace BlazorDemoApp.Data;

using System.Net.Http.Json;
using System.Text.Json;


public class DataService
{
    private readonly HttpClient _httpClient;
    //private readonly ILogger _logger;

    public int CounterValue { get; set; }


    public DataService(HttpClient httpClient /*, ILogger logger*/)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    
    public async Task<WeatherForecast[]?> GetForecastAsync(DateTime startDate)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("http://localhost:7777/WeatherForecast");
        }
        catch (HttpRequestException ex)
        {
            //_logger.LogError("An error occurred: {Message}", ex.Message);
            Console.WriteLine("An error occurred: {0}", ex.Message);
        }
        catch (NotSupportedException ex)
        {
            //_logger.LogError("The content type is not supported: {Message}", ex.Message);
            Console.WriteLine("The content type is not supported: {0}", ex.Message);
        }
        catch (JsonException ex)
        {
            //_logger.LogError("Invalid JSON: {Message}", ex.Message);
            Console.WriteLine("Invalid JSON: {0}", ex.Message);
        }

        return Array.Empty<WeatherForecast>();
    }
    

    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };
    //
    //
    // public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    // {
    //     var rng = new Random();
    //     
    //     return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = startDate.AddDays(index),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[rng.Next(Summaries.Length)]
    //         })
    //         .ToArray());
    // }
}

// http://localhost:7777/WeatherForecast
