/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace BlazorDemoApp.Data;

using System.Net.Http.Json;
using System.Text.Json;


public class DataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DataService> _logger;

    public int CounterValue { get; set; }


    public DataService(HttpClient httpClient, ILogger<DataService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    
    public async Task<WeatherForecast[]?> GetForecastAsync(DateTime startDate)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("http://localhost:7777/WeatherForecast");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("An error occurred: {Message}", ex.Message);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError("The content type is not supported: {Message}", ex.Message);
        }
        catch (JsonException ex)
        {
            _logger.LogError("Invalid JSON: {Message}", ex.Message);
        }

        return Array.Empty<WeatherForecast>();
    }
}

// http://localhost:7777/WeatherForecast
