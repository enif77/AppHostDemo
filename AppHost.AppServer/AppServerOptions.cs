/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;


public class AppServerOptions
{
    /// <summary>
    /// Application args.
    /// </summary>
    public string[] Args { get; set; } = Array.Empty<string>();

    /// <summary>
    /// A port on which this AppServer listens to. 
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Adds middleware for redirecting HTTP Requests to HTTPS.
    /// </summary>
    public bool UseHttpsRedirection { get; init; }

    /// <summary>
    /// Adds the AuthorizationMiddleware to the specified IApplicationBuilder, which enables authorization capabilities.
    /// </summary>
    public bool UseAuthorization { get; init; }

    /// <summary> 
    /// If true, static files hosting is enabled. Disabled (false) by default.
    /// </summary>
    public bool UseStaticFiles { get; init; }

    /// <summary>
    /// A path to a dist directory of an Angular app. Ignored, when the UseStaticFiles is disabled.
    /// </summary>
    public string WebRootPath { get; init; } = string.Empty;
    
    /// <summary>
    /// The list of assemblies containing controllers.
    /// </summary>
    public IList<Assembly> AssembliesWithControllers { get; } = new List<Assembly>();

    /// <summary>
    /// Callback for registering user defined services.
    /// </summary>
    public Action<IServiceCollection>? RegisterServices { get; init; }

    /// <summary>
    /// An optional custom logging configurator.
    /// </summary>
    public ILoggerConfigurator? LoggerConfigurator { get; init; }
}
