using System;
using VirtoCommerce.Platform.Security.OpenIddict;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class ResourceIndicatorValidatorTests
{
    private static readonly Uri PlatformOrigin = new("https://commerce.example:8443");

    [Theory]
    [InlineData("https://commerce.example:8443/ucp/mcp")]
    [InlineData("HTTPS://COMMERCE.EXAMPLE:8443/module/resource")]
    [InlineData("https://commerce.example:8443/module/resource?tenant=one")]
    public void IsSameOriginWebResource_AcceptsPlatformResources(string resource)
    {
        Assert.True(ResourceIndicatorValidator.IsSameOriginWebResource(resource, PlatformOrigin));
    }

    [Theory]
    [InlineData("https://other.example:8443/ucp/mcp")]
    [InlineData("https://commerce.example/ucp/mcp")]
    [InlineData("https://user@commerce.example:8443/ucp/mcp")]
    [InlineData("https://commerce.example:8443/ucp/mcp#fragment")]
    [InlineData("urn:example:resource")]
    [InlineData("commerce.example/ucp/mcp")]
    public void IsSameOriginWebResource_RejectsResourcesOutsidePlatformOrigin(string resource)
    {
        Assert.False(ResourceIndicatorValidator.IsSameOriginWebResource(resource, PlatformOrigin));
    }

    [Fact]
    public void IsSubset_AllowsAudienceNarrowingButRejectsExpansion()
    {
        var granted = new[]
        {
            "https://commerce.example:8443/ucp/mcp",
            "https://commerce.example:8443/another/resource",
        };

        Assert.True(ResourceIndicatorValidator.IsSubset(
            ["HTTPS://COMMERCE.EXAMPLE:8443/ucp/mcp"],
            granted));
        Assert.False(ResourceIndicatorValidator.IsSubset(
            ["https://commerce.example:8443/not-granted"],
            granted));
    }
}
