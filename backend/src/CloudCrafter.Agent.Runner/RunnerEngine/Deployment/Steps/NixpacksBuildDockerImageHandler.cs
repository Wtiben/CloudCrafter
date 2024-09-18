﻿using CloudCrafter.Agent.Models.Configs;
using CloudCrafter.Agent.Models.Deployment;
using CloudCrafter.Agent.Models.Deployment.Steps;
using CloudCrafter.Agent.Models.Deployment.Steps.Params;
using CloudCrafter.Agent.Models.Exceptions;
using CloudCrafter.Agent.Models.Recipe;
using CloudCrafter.Agent.Models.Runner;
using CloudCrafter.Agent.Runner.Cli.Helpers.Abstraction;
using CloudCrafter.Agent.Runner.DeploymentLogPump;

namespace CloudCrafter.Agent.Runner.RunnerEngine.Deployment.Steps;

[DeploymentStep(DeploymentBuildStepType.NixpacksBuildDockerImage)]
// ReSharper disable once UnusedType.Global
public class NixpacksBuildDockerImageHandler(IMessagePump pump, INixpacksHelper nixpacksHelper)
    : IDeploymentStepHandler<NixpacksBuildDockerImageParams>
{
    private readonly IDeploymentLogger _logger =
        pump.CreateLogger<NixpacksBuildDockerImageHandler>();

    public async Task ExecuteAsync(
        NixpacksBuildDockerImageParams parameters,
        DeploymentContext context
    )
    {
        _logger.LogInfo("Building Docker image via Nixpacks");

        var planPath = context.GetRecipeResult<string>(RecipeResultKeys.NixpacksTomlLocation);

        if (string.IsNullOrWhiteSpace(planPath))
        {
            throw new DeploymentException("Nixpacks plan not found - cannot build Docker image.");
        }

        var workDir = context.GetGitDirectory() + $"/git/{parameters.Path}";

        var image = $"{parameters.Image}:{parameters.Tag}";

        var buildEnvironmentVariables = context
            .Recipe.EnvironmentVariables.Variables.Select(x => x.Value)
            .Where(x => x.IsBuildVariable)
            .ToDictionary(x => x.Name, x => x.Value);

        var result = await nixpacksHelper.BuildDockerImage(
            new NixpacksBuildDockerImageConfig
            {
                PlanPath = planPath,
                WorkDir = workDir,
                ImageName = image,
                DisableCache = parameters.DisableCache.GetValueOrDefault(),
                EnvironmentVariables = buildEnvironmentVariables,
            }
        );

        _logger.LogInfo($"Successfully built Docker image: {image}");
    }

    public Task DryRun(NixpacksBuildDockerImageParams parameters, DeploymentContext context)
    {
        _logger.LogInfo("Building Docker image via Nixpacks");

        return Task.CompletedTask;
    }
}
