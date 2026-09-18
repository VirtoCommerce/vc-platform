using Moq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Tests;

public static class ScopedServiceFactoryStub
{
    // A factory that hands out the given instance without a scope, so disposing the wrapper releases nothing.
    public static IScopedServiceFactory<T> Of<T>(T service) where T : class
    {
        var factory = new Mock<IScopedServiceFactory<T>>();
        factory.Setup(x => x.Create()).Returns(() => new ScopedService<T>(service));
        return factory.Object;
    }
}
