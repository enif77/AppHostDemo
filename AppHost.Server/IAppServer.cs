/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Server;

using Microsoft.Extensions.Logging;


/// <summary>
/// An app server.
/// </summary>
public interface IAppServer
{
    /// <summary>
    /// An ILogger instance used by this instance.
    /// </summary>
    ILogger Logger { get; }
    
    /// <summary>
    /// Returns true, if the internal web server task is running.
    /// </summary>
    bool IsRunning { get; }
    
    /// <summary>
    /// Starts the internal web server task.
    /// </summary>
    /// <param name="cancellationTokenSource">An optional CancellationTokenSource instance.</param>
    /// <returns>Awaitable Task instance.</returns>
    Task StartAsync(CancellationTokenSource? cancellationTokenSource = null);
    
    /// <summary>
    /// Stops this instance internal web server task, if it is running.
    /// Throws exception, if the internal task is not running.
    /// </summary>
    /// <returns>A TaskStatus instance representing the status of the finished web server task.</returns>
    Task<TaskStatus> StopAsync();
}