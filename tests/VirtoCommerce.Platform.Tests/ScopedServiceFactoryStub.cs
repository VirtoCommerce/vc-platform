using Moq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests;

public static class ScopedServiceFactoryStub
{
    // A factory that hands out the given instance without a DI scope. The wrapper then owns the instance itself,
    // so disposing the wrapper disposes the instance when it is IDisposable, exactly as the legacy `using var` did.
    public static IScopedServiceFactory<T> Of<T>(T service) where T : class
    {
        var factory = new Mock<IScopedServiceFactory<T>>();
        factory.Setup(x => x.Create()).Returns(() => new ScopedService<T>(service));
        return factory.Object;
    }
}
