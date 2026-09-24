using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Exceptions;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests.DistributedLock;

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
}
