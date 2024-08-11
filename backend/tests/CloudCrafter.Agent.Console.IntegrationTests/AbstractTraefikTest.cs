﻿using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace CloudCrafter.Agent.Console.IntegrationTests;

public abstract class AbstractTraefikTest
{
    protected IContainer TraefikContainer { get; private set; }
    [OneTimeSetUp]
    public async Task Setup()
    {
        TraefikContainer = new ContainerBuilder()
            .WithImage("traefik:v3.1")
            .WithPortBinding(80, 80)
            .WithCommand(
                "--api.insecure=true",
                "--providers.docker=true",
                "--providers.docker.exposedbydefault=false",
                "--entrypoints.web.address=:80"
            )
            .WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
            .WithBindMount("/var/run/docker.sock","/var/run/docker.sock")
            .Build();

        await TraefikContainer.StartAsync();
    }
    
    [OneTimeTearDown]
    public async Task TearDown()
    {
        if (TraefikContainer != null)
        {
            await TraefikContainer.StopAsync();
            await TraefikContainer.DisposeAsync();
        }
    }
}
