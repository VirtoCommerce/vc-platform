using FluentAssertions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security.Authentication;

public class ApiKeyAuthenticationOptionsTests
{
    [Fact]
    public void ApiKeyParamName_NotConfigured_DefaultsToThePlatformConstant()
    {
        // If these two drift apart, anything outside this assembly writes a parameter the handler no
        // longer reads - and nothing fails at build time.
        new ApiKeyAuthenticationOptions().ApiKeyParamName.Should().Be(PlatformConstants.Security.ApiKeyAuthentication.ParamName);
        PlatformConstants.Security.ApiKeyAuthentication.ParamName.Should().Be("api_key");
    }
}
