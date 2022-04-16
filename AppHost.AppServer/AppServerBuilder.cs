/* AppHostDemo - (C) 2022 Premysl Fara  */

using Microsoft.Extensions.Logging;

namespace AppHost.AppServer;

using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
            app.UseStaticFiles();
        }

        if (options.UseAuthorization)
        {
            app.UseAuthorization();
        }

        app.MapControllers();
   
        return new AppServer(app, app.Logger);
    }
}