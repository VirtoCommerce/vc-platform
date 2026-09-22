using System;

namespace VirtoCommerce.Platform.Core.Common;

// Bridges legacy factories to IScopedFactory<T>: a Func<T> whose result the wrapper then owns itself,
// or any delegate that already produces a ScopedService<T>. Used by the [Obsolete] constructors that keep the Func<T> signatures alive.
public sealed class DelegateScopedFactory<T> : IScopedFactory<T>
    where T : class
{
    private readonly Func<ScopedService<T>> _create;

    public DelegateScopedFactory(Func<ScopedService<T>> create)
    {
        _create = create ?? throw new ArgumentNullException(nameof(create));
    }

    public DelegateScopedFactory(Func<T> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _create = () => new ScopedService<T>(factory());
    }

    public ScopedService<T> Create()
    {
        return _create();
    }
}
