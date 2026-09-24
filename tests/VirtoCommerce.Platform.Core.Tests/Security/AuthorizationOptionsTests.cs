using VirtoCommerce.Platform.Core.Security;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Security
{
    public class AuthorizationOptionsTests
    {
        private readonly AuthorizationOptions _options = new()
        {
            OAuthLoginPath = "/oauth/authorize",
            BuiltInLoginHosts = ["platform.example", "Admin.Example"],
        };

        [Fact]
        public void GetOAuthLoginPath_HostWithItsOwnFrontEnd_UsesTheConfiguredPath()
        {
            Assert.Equal("/oauth/authorize", _options.GetOAuthLoginPath("store.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_HostServedByThePlatformItself_SelectsTheBuiltInLoginPage()
        {
            // The Platform host has no front-end of its own to redirect to.
            Assert.Null(_options.GetOAuthLoginPath("platform.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_HostMatchIsCaseInsensitive()
        {
            Assert.Null(_options.GetOAuthLoginPath("admin.example"));
        }

        [Fact]
        public void GetOAuthLoginPath_UnknownHost_DoesNotThrow()
        {
            Assert.Equal("/oauth/authorize", _options.GetOAuthLoginPath(null));
        }

        [Fact]
        public void GetOAuthLoginPath_NoHostsConfigured_KeepsPreviousBehaviour()
        {
            var options = new AuthorizationOptions { OAuthLoginPath = "/oauth/authorize" };

            Assert.Equal("/oauth/authorize", options.GetOAuthLoginPath("store.example"));
        }
    }
}
