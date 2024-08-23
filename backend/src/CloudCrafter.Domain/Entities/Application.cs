﻿using CloudCrafter.Domain.Interfaces;

namespace CloudCrafter.Domain.Entities;

public class Application : IHasTimestamps
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Project Project { get; set; }
    public required Server Server { get; set; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; set; }

    public ApplicationSource? Source { get; set; }

    public List<Deployment> Deployments { get; set; } = new();
    public List<ApplicationService> Services { get; set; } = new();
}
