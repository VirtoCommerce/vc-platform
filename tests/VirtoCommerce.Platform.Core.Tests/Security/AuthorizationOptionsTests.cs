using VirtoCommerce.Platform.Core.Security;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Security
{
    public class AuthorizationOptionsTests
    {
        private readonly AuthorizationOptions _options = new()
        {
            OAuthLoginPath = "/oauth/authorize",
            OAuthLoginPaths =
            {
                ["platform.example"] = string.Empty,
                ["Loyalty.Example"] = "/account/oauth",
            },
        };

        [Fact]
        public void GetOAuthLoginPath_HostWithoutOverride_UsesTheCommonPath()
        {
            Assert.Equal("/oauth/authorize", _options.GetOAuthLoginPath("store.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_HostWithOwnFrontEnd_UsesItsPath()
        {
            Assert.Equal("/account/oauth", _options.GetOAuthLoginPath("loyalty.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_HostServedByThePlatformItself_SelectsTheBuiltInLoginPage()
        {
            // An empty override is the point of the setting: the Platform host has no front-end of
            // its own to redirect to, so it falls back to the built-in login page.
            Assert.Empty(_options.GetOAuthLoginPath("platform.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_UnknownHost_DoesNotThrow()
        {
            Assert.Equal("/oauth/authorize", _options.GetOAuthLoginPath(null));
        }

        [Fact]
        public void GetOAuthLoginPath_NoOverridesConfigured_KeepsPreviousBehaviour()
        {
            var options = new AuthorizationOptions { OAuthLoginPath = "/oauth/authorize" };

            Assert.Equal("/oauth/authorize", options.GetOAuthLoginPath("store.example"));
        }
    }
}
