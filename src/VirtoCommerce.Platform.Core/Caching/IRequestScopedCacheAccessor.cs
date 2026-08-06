namespace VirtoCommerce.Platform.Core.Caching;

/// <summary>
/// Resolves the <see cref="IRequestScopedCache"/> of the ambient request, for consumers whose own lifetime
/// is not bounded by a single request.
/// </summary>
/// <remarks>
/// <see cref="IRequestScopedCache"/> is registered Scoped, so constructor-injecting it is safe only while the
/// consumer's own lifetime never exceeds one request. That does not hold for a singleton, for an object that is
/// cached across requests, or for anything constructed inside a background job's scope - and the DI scope
/// validator cannot tell the difference, because each of those registrations is individually legal. Depend on
/// this accessor instead: it reads the cache from the ambient request every time it is asked, so nothing is
/// captured at construction time.
/// <br/><br/>
/// Consumers also gain no dependency on ASP.NET Core, and become testable with a stub returning a plain
/// <see cref="IRequestScopedCache"/> instead of a fabricated HTTP context.
/// </remarks>
public interface IRequestScopedCacheAccessor
{
    /// <summary>
    /// The ambient request's cache, re-read on every access, or <c>null</c> when there is no ambient request scope.
    /// </summary>
    /// <remarks>
    /// <c>null</c> is an expected value rather than an error: it means a background job, application startup,
    /// or a fabricated context carrying no request services. Outside a request there is nothing to bound a
    /// per-request cache to, so a caller should run its load uncached instead of falling back to a cache with a
    /// wider lifetime. A background job in particular must not deduplicate loads across its own steps, or it
    /// stops observing its own writes for the rest of its run.
    /// <br/><br/>
    /// Work that outlives the response must not read this property. A continuation started inside a request but
    /// not awaited by it still carries the ambient context, whose request scope has since been disposed - the
    /// read then throws <see cref="System.ObjectDisposedException"/> rather than returning <c>null</c>.
    /// That exception is a correct signal about the caller's lifetime and must not be swallowed.
    /// </remarks>
    IRequestScopedCache Cache { get; }
}
