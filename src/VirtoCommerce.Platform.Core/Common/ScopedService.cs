using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

// A service resolved from a DI scope of its own. Disposing the wrapper disposes the scope,
// and with it the service and everything else the scope created.
public sealed class ScopedService<T> : IDisposable, IAsyncDisposable
    where T : class
{
    // Null when no scope owns the service; the wrapper then owns the instance itself.
    private readonly IServiceScope _scope;
    private bool _disposed;

    public ScopedService(T service, IServiceScope scope)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    // Wraps a service that no DI scope owns (legacy Func<T> factories, test doubles):
    // disposing the wrapper disposes the service itself when it is disposable.
    public ScopedService(T service)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public T Service { get; }

    // The scope's provider, for services that must live in the same scope as Service. Null when no scope owns the service.
    public IServiceProvider ServiceProvider => _scope?.ServiceProvider;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        // The scope disposes the service together with everything else it created; a singleton resolved through it is left alone.
        if (_scope != null)
        {
            _disposed = true;
            _scope.Dispose();
            return;
        }

        if (Service is IDisposable disposable)
        {
            _disposed = true;
            disposable.Dispose();
        }
        else if (Service is not IAsyncDisposable)
        {
            _disposed = true;
        }

        // An instance that is only IAsyncDisposable cannot be released synchronously; ownership is kept so DisposeAsync still can.
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (_scope != null)
        {
            if (_scope is IAsyncDisposable asyncScope)
            {
                await asyncScope.DisposeAsync();
            }
            else
            {
                _scope.Dispose();
            }

            return;
        }

        switch (Service)
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
