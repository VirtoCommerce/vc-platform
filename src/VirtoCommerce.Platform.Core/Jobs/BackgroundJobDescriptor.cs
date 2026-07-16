#nullable enable
namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// A registered background-job handler, captured by <c>AddBackgroundJob</c> and registered as a singleton so the full
/// catalog is discoverable via <c>IEnumerable&lt;BackgroundJobDescriptor&gt;</c> (mirrors how
/// <see cref="RecurringJobRegistration"/> makes recurring jobs enumerable). Enables the admin "list registered jobs"
/// view and name-addressed triggering, without reflecting over the DI container.
/// </summary>
public sealed record BackgroundJobDescriptor
{
    /// <summary>Friendly, unique name used to address the job (defaults to the handler's type name).</summary>
    public required string Name { get; init; }

    /// <summary>Assembly-qualified name of the concrete handler type.</summary>
    public required string HandlerType { get; init; }

    /// <summary>Assembly-qualified name of the payload contract type the handler runs.</summary>
    public required string PayloadType { get; init; }

    /// <summary>
    /// Whether this handler may be triggered on demand via the admin/integration API. Internal plumbing handlers
    /// (e.g. the map/reduce coordinators) and sensitive system handlers register with <c>false</c>: they are still
    /// listed for troubleshooting but cannot be run by name with a caller-supplied payload. Defaults to <c>true</c>.
    /// </summary>
    public bool Triggerable { get; init; } = true;
}
