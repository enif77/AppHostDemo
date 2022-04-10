/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.AppServer;

using System.Reflection;


public class AppServerOptions
{
    /// <summary>
    /// Application args.
    /// </summary>
    public string[] Args { get; set; } = Array.Empty<string>();

    /// <summary>
    /// A port on which this AppServer listens to. 
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Adds middleware for redirecting HTTP Requests to HTTPS.
    /// </summary>
    public bool UseHttpsRedirection { get; set; }

    /// <summary>
    /// Adds the AuthorizationMiddleware to the specified IApplicationBuilder, which enables authorization capabilities.
    /// </summary>
    public bool UseAuthorization { get; set; }

    /// <summary> 
    /// If true, static files hosting is enabled. Disabled (false) by default.
    /// </summary>
    public bool UseStaticFiles { get; set; }

    /// <summary>
    /// A path to a dist directory of an Angular app. Ignored, when the UseStaticFiles is disabled.
    /// </summary>
    public string WebRootPath { get; set; } = string.Empty;
    
    /// <summary>
    /// The list of assemblies containing controllers.
    /// </summary>
    public IList<Assembly> AssembliesWithControllers { get; } = new List<Assembly>();

    /// <summary>
    /// An optional custom logging configurator.
    /// </summary>
    public ILoggerConfigurator? LoggerConfigurator { get; set; }
}
