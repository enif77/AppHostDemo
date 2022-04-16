/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Services;

using Microsoft.Extensions.Logging;

using AppHost.Models;


public class LoggingService : ILoggingService
{
    private readonly ILogger<ILoggingService> _logger;
    
    private static readonly string[] LogLevels =
    {
        "trace", "debug", "information", "warning", "error", "critical"
    };
    

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public IEnumerable<string> GetLogLevels()
        => LogLevels;


    public void Log(LogMessage? logMessage)
    {
        if (logMessage == null || string.IsNullOrEmpty(logMessage.Message))
        {
            return;
        }

        var logLevel = logMessage.LogLevel;
        if (string.IsNullOrWhiteSpace(logLevel))
        {
            logLevel = "information";
        }

        switch (logLevel.Trim().ToLowerInvariant())
        {
            case "trace": _logger.LogTrace(logMessage.Message); break;
            case "debug": _logger.LogDebug(logMessage.Message); break;
            
            default:
            // case "information":
                _logger.LogInformation(logMessage.Message); break;
            
            case "warning": _logger.LogWarning(logMessage.Message); break;
            case "error": _logger.LogError(logMessage.Message); break;
            case "critical": _logger.LogCritical(logMessage.Message); break;
        }
    }
}