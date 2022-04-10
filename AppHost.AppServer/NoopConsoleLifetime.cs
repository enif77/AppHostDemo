/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public class NoopConsoleLifetime : IHostLifetime, IDisposable
{
    private readonly ILogger<NoopConsoleLifetime> _logger;

    
    public NoopConsoleLifetime(ILogger<NoopConsoleLifetime> logger)
    {
        _logger = logger;
    }

    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    
    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        Console.CancelKeyPress += OnCancelKeyPressed;
        
        return Task.CompletedTask;
    }

    
    private void OnCancelKeyPressed(object? sender, ConsoleCancelEventArgs e)
    {
        _logger.LogDebug("Ctrl+C has been pressed, ignoring");
        e.Cancel = true;
    }

    
    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancelKeyPressed;
    }
}

// https://stackoverflow.com/questions/68534062/disable-ctrlc-shutdown-in-asp-net-core-web-api-application