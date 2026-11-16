using System.Net;
using FluentAssertions;
using VirtoCommerce.Platform.Security.Extensions;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class IPAddressExtensionsTests
{
    [Theory]
    [InlineData("8.8.8.8")]
    [InlineData("160.79.104.10")]
    [InlineData("2606:4700:4700::1111")]
    [InlineData("::ffff:8.8.8.8")]
    [InlineData("2002:0808:0808::1")]
    [InlineData("64:ff9b::808:808")]
    public void IsPublic_InternetAddress_True(string address)
    {
        // Arrange
        var ipAddress = IPAddress.Parse(address);

        // Act
        var result = ipAddress.IsPublic();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("0.0.0.0")]
    [InlineData("10.1.2.3")]
    [InlineData("100.64.0.1")]
    [InlineData("127.0.0.1")]
    [InlineData("169.254.169.254")]
    [InlineData("172.16.0.1")]
    [InlineData("172.31.255.255")]
    [InlineData("192.168.1.1")]
    [InlineData("192.0.2.1")]
    [InlineData("198.18.0.1")]
    [InlineData("224.0.0.1")]
    [InlineData("255.255.255.255")]
    [InlineData("::")]
    [InlineData("::1")]
    [InlineData("fe80::1")]
    [InlineData("fc00::1")]
    [InlineData("fd12:3456::1")]
    [InlineData("ff02::1")]
    [InlineData("2001:db8::1")]
    [InlineData("64:ff9b:1::a00:1")]
    [InlineData("100::1")]
    [InlineData("2001:0::1")]
    [InlineData("::ffff:127.0.0.1")]
    [InlineData("::ffff:169.254.169.254")]
    [InlineData("::10.0.0.1")]
    [InlineData("2002:0a00:0001::1")]
    [InlineData("64:ff9b::a00:1")]
    public void IsPublic_LocalOrPrivateAddress_False(string address)
    {
        // Arrange
        var ipAddress = IPAddress.Parse(address);

        // Act
        var result = ipAddress.IsPublic();

        // Assert
        result.Should().BeFalse();
    }
}
