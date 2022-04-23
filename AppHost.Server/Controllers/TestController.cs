/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using Microsoft.Extensions.Logging;
    

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        
        [HttpGet(Name = "GetSomething")]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Something returned");
            
            return Summaries.ToArray();
        }
    }    
}
