using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RedLockNet;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

public class StartupLockTests
{
    [Theory]
    [InlineData("VirtoCommerce.Platform.DistributedLock.IInternalDistributedLockService")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.DistributedLockCondition")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.InternalNoLockService")]
    [InlineData("VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService")]
    public void StartupLockTypes_AreObsoleteAndHiddenFromIntelliSense(string typeName)
    {
        var type = typeof(DistributedLockOptions).Assembly.GetType(typeName, throwOnError: true);

        type.GetCustomAttribute<ObsoleteAttribute>().Should().NotBeNull();
        type.GetCustomAttribute<EditorBrowsableAttribute>()!.State.Should().Be(EditorBrowsableState.Never);
    }

    [Fact]
    public void ExecuteSynchronized_WhenLockNotAcquired_NamesResourceInError()
    {
        var redLock = new Mock<IRedLock>();
        redLock.SetupGet(x => x.IsAcquired).Returns(false);
        var factory = new Mock<IDistributedLockFactory>();
        factory.Setup(x => x.CreateLock(It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns(redLock.Object);
        factory.Setup(x => x.CreateLock(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken?>()))
            .Returns(redLock.Object);
#pragma warning disable VC0015 // Testing the Platform startup lock
        var service = new VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService(
            factory.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedLockOptions { WaitTime = 0 }),
            NullLogger<VirtoCommerce.Platform.DistributedLock.Redis.InternalDistributedLockService>.Instance);

        var act = () => service.ExecuteSynchronized("startup-resource", _ => { });
#pragma warning restore VC0015

        act.Should().Throw<PlatformException>().WithMessage("*startup-resource*");
    }
}
