/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Logging;
    
    using AppHost.Models;
    
    
    [ApiController]
    [Route("log")]
    public class LoggingController : ControllerBase
    {
        private static readonly string[] LogLevels =
        {
            "trace", "debug", "information", "warning", "error", "critical"
        };

        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return LogLevels;
        }
        
        [HttpPost]
        // [Consumes("application/x-www-form-urlencoded")]  // -> For FormUrlEncodedContent, not necessary.
        public void Log([FromForm] string? logLevel, [FromForm] string? message)
        {
            if (string.IsNullOrEmpty(logLevel))
            {
                logLevel = "information";
            }

            // TODO: Switch by the logLevel.
            
            _logger.LogInformation("A message '{Message}' at '{LogLevel}' log level received",
                message, logLevel);
        }
        
        [HttpPost]
        [Route("/logMessage")]
        public void LogMessage(LogMessage logMessage)
        {
            if (string.IsNullOrEmpty(logMessage.LogLevel))
            {
                logMessage.LogLevel = "information";
            }

            // TODO: Switch by the logLevel.
            
            _logger.LogInformation("A message '{Message}' at '{LogLevel}' log level received",
                logMessage.Message, logMessage.LogLevel);
        }
    }    
}
