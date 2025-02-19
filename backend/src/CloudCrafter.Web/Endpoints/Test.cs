﻿using CloudCrafter.Agent.Models.SignalR;
using CloudCrafter.Core.Commands;
using CloudCrafter.Core.Interfaces.Domain.Agent;
using CloudCrafter.Core.Interfaces.Domain.Applications.Deployments;
using CloudCrafter.Core.SignalR;
using CloudCrafter.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CloudCrafter.Web.Endpoints;

public class Test : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).MapGet(GetConnections).MapGet(Disconnect, "/disconnect/{id}");
    }

    public object GetConnections(IServiceProvider sp)
    {
        var hubLifetimeManager = sp.GetRequiredService<HubLifetimeManager<AgentHub>>();

        if (hubLifetimeManager is CloudCrafterHubLifetimeManager<AgentHub> manager)
        {
            return manager.GetConnectionIds();
        }

        return new List<string>();
    }

    public Task Disconnect(ConnectedServerManager manager, [FromRoute] Guid id)
    {
        return manager.DisconnectAgent(id);
    }
}
