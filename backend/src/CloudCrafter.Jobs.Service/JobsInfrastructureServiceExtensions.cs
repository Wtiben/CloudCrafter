﻿using CloudCrafter.Jobs.Service.Jobs.Context.Deployments;
using Hangfire;
using Hangfire.Console;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudCrafter.Jobs.Service;

public static class JobsInfrastructureServiceExtensions
{
    public static IServiceCollection AddJobInfrastructure(
        this IServiceCollection services,
        IConfiguration config,
        bool withServer,
        JobServiceType type
    )
    {
        services.AddScoped<IDeploymentTracker, DeploymentTracker>();
        services.AddHangfire(
            (sp, hangfireConfig) =>
            {
                var connectionString = config.GetConnectionString("RedisConnection");
                hangfireConfig.UseRedisStorage(
                    connectionString,
                    new RedisStorageOptions
                    {
                        Prefix = "cloudCrafter:",
                        Db = 0,
                        FetchTimeout = TimeSpan.FromSeconds(1),
                    }
                );
                hangfireConfig.UseFilter(new LogEverythingAttribute());

                hangfireConfig.UseSerilogLogProvider();
                hangfireConfig.UseConsole();

                // Add queue configuration
            }
        );

        if (withServer)
        {
            string[] queues = type == JobServiceType.Worker ? ["worker", "default"] : ["web"];

            services.AddHangfireServer(opt =>
            {
                opt.Queues = queues;
                var random = new Random();
                var randomValue = random.Next(1, 1000);
                opt.ServerName = $"{type}-{randomValue}";
            });
        }

        return services;
    }
}

public enum JobServiceType
{
    Worker,
    Web,
}
