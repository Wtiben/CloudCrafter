﻿using CloudCrafter.Domain.Entities.Jobs;
using CloudCrafter.Domain.Interfaces;

namespace CloudCrafter.Domain.Entities;

public class BackgroundJob : IHasTimestamps
{
    public required Guid Id { get; init; }
    public required string? HangfireJobId { get; set; }
    public required BackgroundJobType Type { get; init; }
    public ServerConnectivityCheckJob? ServerConnectivityCheckJob { get; set; }
    public List<BackgroundJobLog> Logs { get; set; } = new();
    public required BackgroundJobStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}

public enum BackgroundJobType
{
    ServerConnectivityCheck
}

public enum BackgroundJobStatus
{
    Created,
    Enqueued,
    Running,
    Failed,
    Completed
}

public class BackgroundJobLog
{
    public required DateTime Timestamp { get; set; }
    public required string Level { get; set; }
    public required string Message { get; set; }
}
