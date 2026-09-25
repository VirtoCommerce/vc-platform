using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

public class DistributedLockTimeoutExceptionTests
{
    [Fact]
    public void Constructor_SetsResourceTimeoutAndMessage()
    {
        var exception = new DistributedLockTimeoutException("cart:1", TimeSpan.FromSeconds(5));

        exception.Should().BeAssignableTo<PlatformException>();
        exception.Resource.Should().Be("cart:1");
        exception.Timeout.Should().Be(TimeSpan.FromSeconds(5));
        exception.Message.Should().Contain("cart:1").And.Contain("00:00:05");
    }

    [Fact]
    public void DistributedLockOptions_Defaults()
    {
        var options = new VirtoCommerce.Platform.DistributedLock.DistributedLockOptions();

        options.DefaultTimeout.Should().Be(TimeSpan.FromSeconds(30));
        options.Expiry.Should().Be(TimeSpan.FromSeconds(30));
        options.RetryInterval.Should().Be(TimeSpan.FromMilliseconds(100));
        options.MaxRetryInterval.Should().Be(TimeSpan.FromSeconds(2));
        options.StartupExpiry.Should().Be(TimeSpan.FromMinutes(5));
        options.KeyPrefix.Should().BeNull();
        options.WaitTime.Should().Be(180);
    }

    [Fact]
    public void Constructor_WithInnerException_KeepsCause()
    {
        var cause = new TimeoutException("store did not answer");

        var exception = new DistributedLockTimeoutException("cart:1", TimeSpan.FromSeconds(5), cause);

        exception.InnerException.Should().BeSameAs(cause);
        exception.Resource.Should().Be("cart:1");
    }

    [Fact]
    public void StandardConstructors_AreAvailable()
    {
        var cause = new TimeoutException();

        new DistributedLockTimeoutException().Message.Should().NotBeNullOrEmpty();
        new DistributedLockTimeoutException("busy").Message.Should().Be("busy");
        new DistributedLockTimeoutException("busy", cause).InnerException.Should().BeSameAs(cause);
    }
}
