﻿using CloudCrafter.Core.Common.Interfaces;
using CloudCrafter.Core.Interfaces.Domain.Servers;
using CloudCrafter.DeploymentEngine.Engine.Abstraction;
using CloudCrafter.DeploymentEngine.Engine.Brewery.RecipeGenerators;
using CloudCrafter.DeploymentEngine.Remote.Clients.Contracts;
using CloudCrafter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudCrafter.Core.Jobs.Stacks;

public class DeployStackBackgroundJob : BaseDeploymentJob, IJob
{
    private Deployment? _deployment;
    private BackgroundJob? _job;

    public DeployStackBackgroundJob() { }

    public DeployStackBackgroundJob(Guid deploymentId)
    {
        DeploymentId = deploymentId;
    }

    public Guid DeploymentId { get; set; }

    public BackgroundJobType Type => BackgroundJobType.StackDeployment;

    public async Task HandleEntity(IApplicationDbContext context, string jobId)
    {
        _job = await context.Jobs.Where(x => x.HangfireJobId == jobId).FirstOrDefaultAsync();

        if (_job == null)
        {
            throw new ArgumentNullException(nameof(jobId), "Job not found");
        }

        _deployment = await context
            .Deployments.Where(x => x.Id == DeploymentId)
            .Include(x => x.Stack)
            .ThenInclude(x => x.Services)
            .Include(x => x.Stack)
            .ThenInclude(stack => stack.Server)
            .FirstOrDefaultAsync();

        if (_deployment == null || _deployment.Stack == null || _deployment.Stack.Server == null)
        {
            throw new ArgumentNullException(nameof(DeploymentId), "Deployment or stack not found");
        }
    }

    public async Task Handle(
        IServiceProvider serviceProvider,
        IApplicationDbContext context,
        ILoggerFactory loggerFactory,
        string jobId
    )
    {
        var logger = loggerFactory.CreateLogger<DeployStackBackgroundJob>();

        logger.LogDebug(
            "Starting deployment for stack ({StackId}), deploymentId: {DeploymentId}",
            _deployment!.StackId,
            DeploymentId
        );

        var engineManager = GetEngineManager(
            serviceProvider.GetRequiredService<IServerConnectivityService>(),
            _deployment!.Stack.Server!
        );

        var commandGenerator = serviceProvider.GetRequiredService<ICommonCommandGenerator>();

        using var client = engineManager.CreateSshClient();
        logger.LogDebug("Connecting to server ({ServerId})", _deployment.Stack.ServerId);
        await client.ConnectAsync();
        logger.LogDebug("Connected to server!");

        var resultWhoAmI = await client.ExecuteCommandAsync("whoami");
        logger.LogDebug("Using user on remote server: {Result}", resultWhoAmI.Result);

        logger.LogDebug("Brewing recipe...");
        var recipeGenerator = new SimpleAppRecipeGenerator(
            new BaseRecipeGenerator.Args
            {
                Stack = _deployment!.Stack,
                DeploymentId = _deployment!.Id,
            }
        );
        var recipe = recipeGenerator.Generate();
        logger.LogDebug("Recipe brewed!");

        await PullHelperImage(logger, client, commandGenerator);

        var helperContainerId = await CreateDockerContainer(
            logger,
            client,
            commandGenerator,
            DeploymentId
        );

        var recipeFileDetails = await WriteRecipeToFile(
            logger,
            client,
            commandGenerator,
            recipe,
            helperContainerId
        );

        await RunRecipe(logger, client, commandGenerator, helperContainerId, recipeFileDetails);
    }
}
