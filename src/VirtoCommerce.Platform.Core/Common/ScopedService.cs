using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

// A service resolved from a DI scope of its own. Disposing the wrapper disposes the scope,
// and with it the service and everything else the scope created.
public sealed class ScopedService<T> : IDisposable, IAsyncDisposable
    where T : class
{
    private IServiceScope _scope;

    public ScopedService(T service, IServiceScope scope)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    // For test doubles and adapters: wraps an instance that no scope owns, so Dispose releases nothing.
    public ScopedService(T service)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public T Service { get; }

    // The scope's provider, for services that must live in the same scope as Service.
    public IServiceProvider ServiceProvider => _scope?.ServiceProvider;

    public void Dispose()
    {
        var scope = _scope;
        _scope = null;
        scope?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        var scope = _scope;
        _scope = null;

        if (scope is IAsyncDisposable asyncDisposable)
        {
            return asyncDisposable.DisposeAsync();
        }

        scope?.Dispose();
        return default;
    }
}
