/* AppHostDemo - (C) 2022 Premysl Fara  */

using Microsoft.Extensions.Logging;

using Serilog;

using AppHost;
using AppHost.AppServer;
using AppHost.Features.Controllers;


// Console.WriteLine("The app is running. Press ENTER to start the app server...");
// _ = Console.ReadLine();  // This represents a running application (the launcher) before it runs the app itself.


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var appServerOptions = new AppServerOptions()
{
    Args = args,
    LoggerConfigurator = new ConsoleLoggerConfigurator(),
    Port = 9999
};

// Add controller(s) from the features project.
appServerOptions.AssembliesWithControllers.Add(typeof(FeaturesTestController).Assembly);

var appServer = AppServerBuilder.Build(appServerOptions);
var logger = appServer.Logger;

logger.LogInformation("Starting the app server...");

await appServer.StartAsync();

logger.LogInformation("The app server is started. Press ENTER to stop the app server...");

_ = Console.ReadLine();  // This represents a running application (the launcher).

logger.LogInformation("Stopping the app server...");
    
var status = await appServer.StopAsync();
    
logger.LogDebug("Task status: {Status}", status);

logger.LogInformation("DONE!");

/*
 
https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
  
https://blog.datalust.co/using-serilog-in-net-6/   
https://github.com/serilog/serilog-extensions-logging  
  
 */
 