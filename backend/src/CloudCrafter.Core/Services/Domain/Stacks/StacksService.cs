﻿using AutoMapper;
using CloudCrafter.Agent.SignalR.Models;
using CloudCrafter.Core.Interfaces.Domain.Stacks;
using CloudCrafter.Core.Interfaces.Repositories;
using CloudCrafter.Domain.Domain.Deployment;
using CloudCrafter.Domain.Domain.Stack;
using CloudCrafter.Domain.Entities;

namespace CloudCrafter.Core.Services.Domain.Stacks;

public class StacksService(IStackRepository repository, IMapper mapper) : IStacksService
{
    public async Task<StackCreatedDto> CreateStack(CreateStackArgsDto args)
    {
        var createdStack = await repository.CreateStack(args);

        return new StackCreatedDto { Id = Guid.NewGuid() };
    }

    public async Task<SimpleStackDetailsDto?> GetSimpleStackDetails(Guid id)
    {
        var stack = await repository.GetStack(id);

        if (stack == null)
        {
            return null;
        }

        return mapper.Map<SimpleStackDetailsDto>(stack);
    }

    public async Task<StackDetailDto?> GetStackDetail(Guid id)
    {
        var stack = await repository.GetStack(id);

        if (stack == null)
        {
            return null;
        }

        return mapper.Map<StackDetailDto>(stack);
    }

    public Task<Guid> CreateDeployment(Guid stackId)
    {
        return repository.CreateDeployment(stackId);
    }

    public async Task<List<SimpleDeploymentDto>> GetDeployments(Guid stackId)
    {
        var deployments = await repository.GetDeployments(stackId);

        return mapper.Map<List<SimpleDeploymentDto>>(deployments);
    }

    public async Task HandleHealthChecks(Guid serverId, ContainerHealthCheckArgs args)
    {
        foreach (var stackInfo in args.Info)
        {
            var stackId = stackInfo.Key;

            var stackEntity = await repository.GetStack(stackId);

            if (stackEntity == null || stackEntity.ServerId != serverId)
            {
                continue;
            }

            var allAreHealthy = stackInfo.Value.StackServices.All(x =>
                x.Value.Status == ContainerHealthCheckStackInfoHealthStatus.Healthy
            );

            var allAreUnhealthy = stackInfo.Value.StackServices.All(x =>
                x.Value.Status == ContainerHealthCheckStackInfoHealthStatus.Unhealthy
            );
            stackEntity.HealthStatus.SetStatus(
                allAreHealthy ? EntityHealthStatusValue.Healthy
                : allAreUnhealthy ? EntityHealthStatusValue.Unhealthy
                : EntityHealthStatusValue.Degraded
            );

            foreach (var stackService in stackInfo.Value.StackServices)
            {
                var stackServiceId = stackService.Key;

                // TODO: Move this to Unit of Work
                var stackServiceEntity = await repository.GetService(stackServiceId);

                if (stackServiceEntity?.Stack.ServerId != serverId)
                {
                    continue;
                }

                var isRunning = stackService.Value.IsRunning;

                stackServiceEntity?.HealthStatus.SetStatus(
                    stackService.Value.Status == ContainerHealthCheckStackInfoHealthStatus.Healthy
                            ? EntityHealthStatusValue.Healthy
                        : stackService.Value.Status
                        == ContainerHealthCheckStackInfoHealthStatus.Unhealthy
                            ? EntityHealthStatusValue.Unhealthy
                        : EntityHealthStatusValue.Degraded,
                    isRunning
                );
            }
        }

        await repository.SaveChangesAsync();
    }
}
