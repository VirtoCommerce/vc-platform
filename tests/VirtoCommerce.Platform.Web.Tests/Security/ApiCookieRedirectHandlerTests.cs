using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    public class ApiCookieRedirectHandlerTests
    {
        private const string LoginUri = "https://localhost:5001/?ReturnUrl=%2Fapi%2Fplatform%2Fsettings";

        [Theory]
        // The regression: API calls from the admin SPA must not be redirected to the login page.
        [InlineData("/api/platform/settings/VirtoCommerce.Platform.Security.AccountTypes")]
        [InlineData("/api/platform/security/users/search")]
        // Path matching is case-insensitive.
        [InlineData("/API/platform/settings")]
        // The prefix itself is an API request.
        [InlineData("/api")]
        public async Task HandleAsync_ApiPath_SetsStatusCodeAndDoesNotRedirect(string path)
        {
            var context = CreateContext(path);

            await ApiCookieRedirectHandler.HandleAsync(context, StatusCodes.Status401Unauthorized);

            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            context.Response.Headers.Location.Should().BeEmpty();
        }

        [Fact]
        public async Task HandleAsync_ApiPath_AccessDenied_SetsForbidden()
        {
            var context = CreateContext("/api/platform/security/users/search");

            await ApiCookieRedirectHandler.HandleAsync(context, StatusCodes.Status403Forbidden);

            context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            context.Response.Headers.Location.Should().BeEmpty();
        }

        [Fact]
        public async Task HandleAsync_AjaxRequestOutsideApiPath_SetsStatusCode()
        {
            // SignalR marks hub negotiation as an AJAX request. It relied on the framework's built-in
            // AJAX special case, which this handler replaces - the 401 must be preserved.
            var context = CreateContext("/pushNotificationHub/negotiate", isAjax: true);

            await ApiCookieRedirectHandler.HandleAsync(context, StatusCodes.Status401Unauthorized);

            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Theory]
        // Browser navigation must keep redirecting, the OpenID Connect authorization flow depends on it.
        [InlineData("/connect/authorize")]
        [InlineData("/")]
        // "apiary" is not the "/api" segment.
        [InlineData("/apiary/docs")]
        public async Task HandleAsync_NonApiPath_RedirectsToLoginPage(string path)
        {
            var context = CreateContext(path);

            await ApiCookieRedirectHandler.HandleAsync(context, StatusCodes.Status401Unauthorized);

            context.Response.StatusCode.Should().Be(StatusCodes.Status302Found);
            context.Response.Headers.Location.ToString().Should().Be(LoginUri);
        }

        private static RedirectContext<CookieAuthenticationOptions> CreateContext(string path, bool isAjax = false)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = path;

            if (isAjax)
            {
                httpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            }

            var scheme = new AuthenticationScheme(IdentityConstants.ApplicationScheme, displayName: null, typeof(CookieAuthenticationHandler));

            return new RedirectContext<CookieAuthenticationOptions>(httpContext, scheme, new CookieAuthenticationOptions(), new AuthenticationProperties(), LoginUri);
        }
    }
}
