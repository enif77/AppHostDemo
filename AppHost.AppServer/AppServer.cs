/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using System.Text;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public class AppServer : IAppServer
{
    private readonly IHost _app;
    private Task? _appServerTask;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _tokenSourceCanBeDisposed;

    public ILogger Logger { get; }

    public bool IsRunning
        => _appServerTask != null &&
           _appServerTask.IsCompleted == false &&
           (_appServerTask.Status == TaskStatus.Running ||
            _appServerTask.Status == TaskStatus.WaitingToRun ||
            _appServerTask.Status == TaskStatus.WaitingForActivation);

    public AppServer(IHost app, ILogger logger)
    {
        _app = app ?? throw new ArgumentNullException(nameof(app));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task StartAsync(CancellationTokenSource? cancellationTokenSource = null)
    {
        if (IsRunning) throw new InvalidOperationException("The app server is already running.");

        if (cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _tokenSourceCanBeDisposed = true;
        }
        else
        {
            _cancellationTokenSource = cancellationTokenSource;
        }
        
        // https://docs.microsoft.com/cs-cz/dotnet/api/system.threading.tasks.task.run?view=net-6.0
        var token = _cancellationTokenSource.Token;

        var t = Task.Run(async () => 
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            // https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebApplication.cs
            // https://github.com/aspnet/Hosting/blob/master/src/Microsoft.Extensions.Hosting.Abstractions/HostingAbstractionsHostExtensions.cs
            await _app.RunAsync(token);
            
            Logger.LogInformation("The app server stopped");
        }, token);

        await Task.Yield();

        _appServerTask = t;
    }


    public async Task<TaskStatus> StopAsync()
    {
        if (_cancellationTokenSource == null) throw new InvalidOperationException("The cancellation token source is not set.");
        if (IsRunning == false) throw new InvalidOperationException("The app server is not running.");

        _cancellationTokenSource.Cancel();

        try
        {
            await _appServerTask!;  // IsRunning checks for null!
        }
        catch (AggregateException e)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("Exception messages:");

            foreach (var ie in e.InnerExceptions)
            {
                sb.AppendLine($"   {ie.GetType().Name}: {ie.Message}");
            }
            
            Logger.LogError(sb.ToString());
        }
        finally
        {
            if (_tokenSourceCanBeDisposed)
            {
                _cancellationTokenSource.Dispose();
            }
        }

        var status = _appServerTask!.Status;  // IsRunning checks for null!

        _appServerTask = null;
        _cancellationTokenSource = null;
        _tokenSourceCanBeDisposed = false;
        
        return status;
    }
}