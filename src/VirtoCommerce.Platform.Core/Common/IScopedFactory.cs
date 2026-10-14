namespace VirtoCommerce.Platform.Core.Common;

// Creates T in a DI scope of its own, for consumers that have no request scope (singletons, event handlers, background jobs).
// Registered once as an open generic, so any T can be requested without a per-type registration.
public interface IScopedFactory<T>
    where T : class
{
    ScopedService<T> Create();
}
