using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Testing;

public static class ScopedFactoryStub
{
    // A factory that hands out the given instance without a DI scope. The wrapper then owns the instance itself,
    // so disposing the wrapper disposes the instance when it is IDisposable, exactly as the legacy `using var` did.
    public static IScopedFactory<T> Of<T>(T service) where T : class
    {
        return new DelegateScopedFactory<T>(() => service);
    }
}
