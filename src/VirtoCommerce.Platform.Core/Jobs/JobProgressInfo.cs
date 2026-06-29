#nullable enable
namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>A single progress update emitted by a running job.</summary>
public sealed record JobProgressInfo
{
    public string? Message { get; init; }

    public long? ProcessedCount { get; init; }

    public long? TotalCount { get; init; }
}
