/* AppHostDemo - (C) 2022 Premysl Fara  */

using Microsoft.Extensions.Logging;

using AppHost.Server;


var appServer = AppServerBuilder.Build(new AppServerOptions()
{
    Args = args,
    Port = 7777,
    UseHttpsRedirection = false,  // TODO: This does not work!
    UseStaticFiles = true,
    SupportBlazorWebAssembly = true,
    WebRootPath = "../../../../BlazorDemoApp/bin/Release/net6.0/publish/wwwroot"
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
