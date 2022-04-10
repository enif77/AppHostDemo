/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using System.Text;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public class AppServer : IAppServer
{
    private readonly IHost _app;
    private Task? _appServerTask;
    private CancellationTokenSource? _tokenSource;
    private bool _tokenSourceCanBeDisposed;

    public ILogger Logger { get; }


    public AppServer(IHost app, ILogger logger)
    {
        _app = app ?? throw new ArgumentNullException(nameof(app));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task StartAsync(CancellationTokenSource? tokenSource = null)
    {
        if (_appServerTask != null) throw new InvalidOperationException("The app server is already running.");

        if (tokenSource == null)
        {
            _tokenSource = new CancellationTokenSource();
            _tokenSourceCanBeDisposed = true;
        }
        else
        {
            _tokenSource = tokenSource;
        }
        
        // https://docs.microsoft.com/cs-cz/dotnet/api/system.threading.tasks.task.run?view=net-6.0
        var token = _tokenSource.Token;

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
        if (_tokenSource == null) throw new InvalidOperationException("The cancellation token source is not set.");
        if (_appServerTask == null) throw new InvalidOperationException("The app server is not running.");
        
        _tokenSource.Cancel();

        try
        {
            await _appServerTask;
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
                _tokenSource.Dispose();
            }
        }

        var status = _appServerTask.Status;

        _appServerTask = null;
        _tokenSource = null;
        _tokenSourceCanBeDisposed = false;
        
        return status;
    }
}