/* AppHostDemo - (C) 2022 Premysl Fara  */

using Microsoft.Extensions.Logging;

using AppHost.AppServer;


var appServer = AppServerBuilder.Build(new AppServerOptions()
{
    Args = args,
    Port = 7777,
    UseHttpsRedirection = true,
    UseStaticFiles = true,
    WebRootPath = "../../../wwwroot/"
});

var logger = appServer.Logger;

logger.LogInformation("Starting the app server...");

await appServer.StartAsync();


Console.WriteLine("Press ENTER to stop...");
_ = Console.ReadLine();


logger.LogInformation("Stopping the app server...");
    
var status = await appServer.StopAsync();
    
logger.LogInformation("Task status: {Status}", status);

logger.LogInformation("DONE!");
