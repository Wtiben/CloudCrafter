﻿using CloudCrafter.Core;
using CloudCrafter.Core.Common.Interfaces;
using CloudCrafter.DeploymentEngine.Remote;
using CloudCrafter.Infrastructure;
using CloudCrafter.Infrastructure.Logging;
using CloudCrafter.Jobs.Service;
using CloudCrafter.Worker.Console.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace CloudCrafter.Worker.Console;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        return await RunApp(args);
    }

    public static async Task<int> RunApp(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var host = builder.Build();

        var validator = host.Services.GetRequiredService<IStartupValidator>();
        validator.Validate();


        await host.RunAsync();
        return 0;
    }


    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();


        var builder = Host.CreateDefaultBuilder(args);

      


        return builder
            .ConfigureAppConfiguration((ctx, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
                config.AddJsonFile("appsettings.Development.json", optional: true);
                config.AddEnvironmentVariables();
                config.AddConfiguration(config.Build());
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                });
                


                services.AddScoped<IUser, NullUser>();

                services.AddEngineInfrastructure();
                services.AddCloudCrafterConfiguration();
                services.AddCloudCrafterLogging(hostContext.Configuration);
                services.AddInfrastructureServices(hostContext.Configuration);
                services.AddJobInfrastructure(hostContext.Configuration, true, "worker");
                services.AddApplicationServices();
            });
    }
}
