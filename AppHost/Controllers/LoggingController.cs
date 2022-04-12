/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Logging;
    
    
    [ApiController]
    [Route("log")]
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
        
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public void Log([FromForm] string? logLevel, [FromForm] string? message)
        {
            if (string.IsNullOrEmpty(logLevel))
            {
                logLevel = "information";
            }

            _logger.LogInformation("A message '{Message}' at '{LogLevel}' log level received",
                message, logLevel);
        }
    }    
}
