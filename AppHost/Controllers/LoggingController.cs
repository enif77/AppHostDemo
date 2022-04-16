/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using AppHost.Models;
    using AppHost.Services;
    
    
    [ApiController]
    [Route("log")]
    public class LoggingController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        
        public LoggingController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _loggingService.GetLogLevels();
        }
        
        // [HttpPost]
        // // [Consumes("application/x-www-form-urlencoded")]  // -> For FormUrlEncodedContent, not necessary.
        // public void Log([FromForm] string? logLevel, [FromForm] string? message)
        // {
        //     _loggingService.Log(new LogMessage()
        //         {
        //             LogLevel = logLevel,
        //             Message = message
        //         });
        // }
        
        [HttpPost]
        //[Route("/log")]
        public void LogMessage(LogMessage logMessage)
        {
            _loggingService.Log(logMessage);
        }
    }    
}
