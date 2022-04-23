/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Server;

using Microsoft.Extensions.Logging;


/// <summary>
/// Custom logger configurator.
/// </summary>
public interface ILoggerConfigurator
{
    /// <summary>
    /// Gets the minimum log level.
    /// </summary>
    LogLevel MinimalLogLevel { get; }
    
    /// <summary>
    /// Configures and adds a custom logger.
    /// </summary>
    /// <param name="builder">An ILoggingBuilder instance.</param>
    /// <returns>The received ILoggingBuilder instance.</returns>
    ILoggingBuilder Configure(ILoggingBuilder builder);
}
