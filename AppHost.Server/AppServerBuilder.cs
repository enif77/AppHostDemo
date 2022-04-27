/* AppHostDemo - (C) 2022 Premysl Fara  */

namespace AppHost.Server;

using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public static class AppServerBuilder
{
    public static IAppServer Build(AppServerOptions? options = null)
    {
        options ??= new AppServerOptions();

        string? webRootPath = null;
        if (options.UseStaticFiles)
        {
            webRootPath = string.IsNullOrWhiteSpace(options.WebRootPath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
                : options.WebRootPath;
        }

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = options.Args,
            WebRootPath = webRootPath
        });

        // A specific port should be used?
        if (options.Port > 0)
        {
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                if (options.UseHttpsRedirection)
                {
                    serverOptions.Listen(IPAddress.Loopback, options.Port, listenOptions =>
                    {
                        listenOptions.UseHttps();
                    });
                }
                else
                {
                    serverOptions.Listen(IPAddress.Loopback, options.Port);
                }
            });
        }
        
        
        // Add custom logging?
        if (options.LoggerConfigurator != null)
        {
            options.LoggerConfigurator.Configure(builder.Logging);
            builder.WebHost.ConfigureLogging(webHostBuilder =>
            {
                // https://stackoverflow.com/questions/53457386/asp-net-core-logging-too-verbose
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/diagnostics?view=aspnetcore-6.0
                webHostBuilder
                    .AddFilter(logLevel => logLevel >= options.LoggerConfigurator.MinimalLogLevel);
                // webHostBuilder
                //     .AddFilter(logLevel => logLevel >= LogLevel.Warning);
            });
        }
        
        
        // Add services to the container.

        var mvcBuilder = builder.Services.AddControllers()
            .AddApplicationPart(typeof(AppServerBuilder).Assembly); // This adds controllers from this assembly.

        // This adds controllers from user defined assemblies.
        foreach (var assembly in options.AssembliesWithControllers)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }
        
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    
        // This removes the CTRL+C app shutdown.
        builder.Services.AddSingleton<IHostLifetime, NoopConsoleLifetime>();

        // Register user defined services to the DI container.
        options.RegisterServices?.Invoke(builder.Services);

        var app = builder.Build();
            
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (options.UseHttpsRedirection)
        {
            app.UseHttpsRedirection();
        }

        if (options.UseStaticFiles)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0
            app.UseDefaultFiles();

            // Blazor is supported when static files support is on only.
            if (options.SupportBlazorWebAssembly)
            {
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0
                
                // Set up custom content types for Blazor. See a web.config/staticContent of a WebAssembly Blazor app.
                var provider = new FileExtensionContentTypeProvider();

                provider.Mappings.Remove(".blat");
                provider.Mappings.Remove(".dat"); 
                provider.Mappings.Remove(".dll"); 
                provider.Mappings.Remove(".json");
                provider.Mappings.Remove(".wasm");
                provider.Mappings.Remove(".woff");
                provider.Mappings.Remove(".woff2");
            
                provider.Mappings[".blat"] = "application/octet-stream";
                provider.Mappings[".dll"] = "application/octet-stream";
                provider.Mappings[".dat"] = "application/octet-stream";
                provider.Mappings[".json"] = "application/json";
                provider.Mappings[".wasm"] = "application/wasm";
                provider.Mappings[".woff"] = "application/font-woff";
                provider.Mappings[".woff2"] = "application/font-woff"; 
            
                app.UseStaticFiles(new StaticFileOptions
                {
                    ContentTypeProvider = provider
                });
            }
            else
            {
                app.UseStaticFiles();
            }
        }

        if (options.UseAuthorization)
        {
            app.UseAuthorization();
        }

        // Blazor is supported when static files support is on only.
        if (options.UseStaticFiles && options.SupportBlazorWebAssembly)
        {
            // Blazor apps need this to properly map routes to sub-pages like https://127.0.0.1:7777/fetchdata.
            // https://docs.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly?view=aspnetcore-6.0
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/{*path:nonfile}", "index.html");
            });
        }
        else
        {
            app.MapControllers();
        }

        // Minimal API demo.
        // https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code
        app.MapGet("/hello", () => "Hello, World!");
        
        return new AppServer(app, app.Logger);
    }
}