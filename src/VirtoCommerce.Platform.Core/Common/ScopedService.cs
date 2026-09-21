using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

// A service resolved from a DI scope of its own. Disposing the wrapper disposes the scope,
// and with it the service and everything else the scope created.
public sealed class ScopedService<T> : IDisposable, IAsyncDisposable
    where T : class
{
    // The DI scope, or the service itself when no scope owns it.
    private object _owned;

    public ScopedService(T service, IServiceScope scope)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
        _owned = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    // Wraps a service that no DI scope owns (legacy Func<T> factories, test doubles):
    // disposing the wrapper disposes the service itself when it is disposable.
    public ScopedService(T service)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
        _owned = service;
    }

    public T Service { get; }

    // The scope's provider, for services that must live in the same scope as Service. Null when no scope owns the service.
    public IServiceProvider ServiceProvider => (_owned as IServiceScope)?.ServiceProvider;

    public void Dispose()
    {
        var owned = _owned;
        _owned = null;

        switch (owned)
        {
            case IDisposable disposable:
                disposable.Dispose();
                break;
            case IAsyncDisposable asyncDisposable:
                asyncDisposable.DisposeAsync().AsTask().GetAwaiter().GetResult();
                break;
        }
    }

    public async ValueTask DisposeAsync()
    {
        var owned = _owned;
        _owned = null;

        switch (owned)
        {
            case IAsyncDisposable asyncDisposable:
                await asyncDisposable.DisposeAsync();
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }
}
