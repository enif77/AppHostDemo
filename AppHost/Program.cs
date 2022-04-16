/* AppHostDemo - (C) 2022 Premysl Fara  */

using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using AppHost;
using AppHost.AppServer;
using AppHost.Features.Controllers;
using AppHost.Services;


// Console.WriteLine("The app is running. Press ENTER to start the app server...");
// _ = Console.ReadLine();  // This represents a running application (the launcher) before it runs the app itself.


// The AppServer configuration.
var appServerOptions = new AppServerOptions()
{
    Args = args,
    LoggerConfigurator = new ConsoleLoggerConfigurator(),
    Port = 9999,
    RegisterServices = services =>
    {
        services.AddSingleton<ILoggingService, LoggingService>();
    }
};

// Add controller(s) from the features project.
appServerOptions.AssembliesWithControllers.Add(typeof(FeaturesTestController).Assembly);

// Build the AppServer.
var appServer = AppServerBuilder.Build(appServerOptions);

// We use the same logger, as the AppServer we just build.
var logger = appServer.Logger;


logger.LogInformation("Starting the app server...");

await appServer.StartAsync();


logger.LogInformation("Starting the client process...");

using (var clientProcess = new Process())
{
    clientProcess.StartInfo.CreateNoWindow = true;
    clientProcess.StartInfo.UseShellExecute = false;
    clientProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    clientProcess.StartInfo.FileName =
        "../../../../AppHost.Client/bin/Release/net6.0/publish/AppHost.Client";
    clientProcess.StartInfo.Arguments = "-p=9999 -r=5";

    clientProcess.Start();
    clientProcess.WaitForExit();
    
    Console.WriteLine("The client process exited with the {0} exit code.", clientProcess.ExitCode);
}

logger.LogInformation("Stopping the app server...");
    
var status = await appServer.StopAsync();
    
logger.LogInformation("Task status: {Status}", status);

logger.LogInformation("DONE!");

/*
 
https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
  
https://blog.datalust.co/using-serilog-in-net-6/   
https://github.com/serilog/serilog-extensions-logging  
  
 */
 