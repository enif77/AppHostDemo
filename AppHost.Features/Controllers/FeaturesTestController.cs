/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Features.Controllers;

using Microsoft.AspNetCore.Mvc;
    
using Microsoft.Extensions.Logging;
    

[ApiController]
[Route("[controller]")]
public class FeaturesTestController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<FeaturesTestController> _logger;

    public FeaturesTestController(ILogger<FeaturesTestController> logger)
    {
        _logger = logger;
    }

        
    [HttpGet(Name = "GetSomethingAgain")]
    public IEnumerable<string> Get()
    {
        _logger.LogInformation("Something returned again");
            
        return Summaries.ToArray();
    }
}   

