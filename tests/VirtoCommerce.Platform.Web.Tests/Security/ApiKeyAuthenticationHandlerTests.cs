using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class ApiKeyAuthenticationHandlerTests
{
    private const string ApiKey = "vc_the-key-the-client-sends-0123456789";

    private readonly Mock<IUserApiKeyService> _userApiKeyService = new();

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public async Task KeyInTheQueryString_IsLookedUpOnlyWhenAllowed(bool allowQueryString, int expectedLookups)
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?api_key=" + ApiKey);

        var result = await AuthenticateAsync(context, allowQueryString);

        result.None.Should().BeTrue("the service knows no key, and a refused query string leaves the request to other schemes");
        _userApiKeyService.Verify(x => x.GetApiKeyByKeyAsync(ApiKey), Times.Exactly(expectedLookups));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task KeyInTheHeader_IsLookedUpWhateverTheQueryStringSetting(bool allowQueryString)
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["api_key"] = ApiKey;

        await AuthenticateAsync(context, allowQueryString);

        _userApiKeyService.Verify(x => x.GetApiKeyByKeyAsync(ApiKey), Times.Once);
    }

    [Fact]
    public async Task KeyInBoth_WithQueryStringRefused_UsesTheHeader()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?api_key=from-the-query-string");
        context.Request.Headers["api_key"] = ApiKey;

        await AuthenticateAsync(context, allowQueryString: false);

        _userApiKeyService.Verify(x => x.GetApiKeyByKeyAsync(ApiKey), Times.Once);
        _userApiKeyService.Verify(x => x.GetApiKeyByKeyAsync("from-the-query-string"), Times.Never);
    }

    private async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, bool allowQueryString)
    {
        var options = new ApiKeyAuthenticationOptions { AllowQueryString = allowQueryString };
        var optionsMonitor = new Mock<IOptionsMonitor<ApiKeyAuthenticationOptions>>();
        optionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

        // The key is unknown, so the handler stops before it needs the user or sign-in managers.
        var handler = new ApiKeyAuthenticationHandler(optionsMonitor.Object, NullLoggerFactory.Instance, UrlEncoder.Default,
            userManager: null, signInManager: null, _userApiKeyService.Object);

        await handler.InitializeAsync(new AuthenticationScheme(ApiKeyAuthenticationOptions.DefaultScheme, null, typeof(ApiKeyAuthenticationHandler)), context);

        return await handler.AuthenticateAsync();
    }
}
