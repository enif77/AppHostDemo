/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost;

using Microsoft.Extensions.Logging;

using AppHost.AppServer;


public class ConsoleLoggerConfigurator : ILoggerConfigurator
{
    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    public LogLevel MinimalLogLevel { get; set; } = LogLevel.Information;

    
    public ILoggingBuilder Configure(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        
        builder.AddConsole();
        builder.SetMinimumLevel(MinimalLogLevel);

        return builder;
    }
}