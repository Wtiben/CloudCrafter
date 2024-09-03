﻿using CloudCrafter.Core.Jobs.Dispatcher.Factory;
using CloudCrafter.Core.Jobs.Servers;
using CloudCrafter.Core.Jobs.Stacks;
using CloudCrafter.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CloudCrafter.Core.Jobs.Dispatcher;

public class CloudCrafterDispatcher(BackgroundJobFactory jobFactory, ILogger<CloudCrafterDispatcher> logger)
    : ICloudCrafterDispatcher
{
    public async Task<string> EnqueueConnectivityCheck(Server server)
    {
        logger.LogDebug("Dispatching connectivity check to job factory for server {ServerName}", server.Name);
        return await jobFactory.CreateAndEnqueueJobAsync<ConnectivityCheckBackgroundJob, Server>(server);
    }

    public async Task EnqueueConnectivityCheck(List<Server> servers)
    {
        foreach (var server in servers)
        {
            await EnqueueConnectivityCheck(server);
        }
    }

    public async Task<string> EnqueueStackDeployment(Guid stackId)
    {
        logger.LogInformation("Dispatching stack deployment to job factory for stack {StackId}", stackId);
        return await jobFactory.CreateAndEnqueueJobAsync<DeployStackBackgroundJob, Guid>(stackId);
    }
}
