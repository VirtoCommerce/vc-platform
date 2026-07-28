#nullable enable
namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// The outcome of a single map item, handed to the reduce step. <see cref="Value"/> is null when
/// <see cref="Succeeded"/> is false; <see cref="Error"/> then carries the failure message.
/// </summary>
/// <typeparam name="TResult">The map result type.</typeparam>
public sealed class MapResult<TResult>
    where TResult : class
{
    public MapResult(int index, TResult? value, bool succeeded, string? error)
    {
        Index = index;
        Value = value;
        Succeeded = succeeded;
        Error = error;
    }

    /// <summary>Zero-based position of the item within the batch.</summary>
    public int Index { get; }

    /// <summary>The map result, or null when the item failed.</summary>
    public TResult? Value { get; }

    public bool Succeeded { get; }

    /// <summary>Failure message when <see cref="Succeeded"/> is false.</summary>
    public string? Error { get; }
}
