/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using Microsoft.Extensions.Logging;


public interface IAppServer
{
    ILogger Logger { get; }
    Task StartAsync(CancellationTokenSource? tokenSource = null);
    Task<TaskStatus> StopAsync();
}