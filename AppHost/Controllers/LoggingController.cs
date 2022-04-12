/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Logging;
    
    using AppHost.Models;
    
    
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private static readonly string[] LogLevels = new[]
        {
            "trace", "debug", "information", "warning", "error", "critical"
        };

        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLogLevels")]
        public IEnumerable<string> Get()
        {
            return LogLevels;
        }
        
        [HttpPost(Name = "Log")]
        public void Log(string? logLevel, string? message)
        {
            if (string.IsNullOrEmpty(logLevel))
            {
                logLevel = "information";
            }

            _logger.LogInformation("A message '{Message}' at '{LogLevel}' log level received", message, logLevel);
        }
    }    
}
