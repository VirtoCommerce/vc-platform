using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

/// <summary>
/// A consent form whose antiforgery token is refused is shown again with a notice, instead of an empty 400.
/// </summary>
public class ShowConsentAgainOnExpiredFormAttributeTests
{
    private readonly ShowConsentAgainOnExpiredFormAttribute _filter = new();

    [Fact]
    public async Task RefusedToken_GoesBackToTheSameAuthorizationRequest()
    {
        var context = CreateContext(new AntiforgeryValidationFailedResult());

        await _filter.OnResultExecutionAsync(context, () => Task.FromResult<ResultExecutedContext>(null));

        var redirect = Assert.IsType<LocalRedirectResult>(context.Result);
        Assert.Equal("/connect/authorize?client_id=claude&scope=openid%20profile&state=opaque", redirect.Url);
    }

    [Fact]
    public async Task RefusedToken_MarksTheRequestForTheConsentPage()
    {
        var context = CreateContext(new AntiforgeryValidationFailedResult());

        await _filter.OnResultExecutionAsync(context, () => Task.FromResult<ResultExecutedContext>(null));

        var cookie = context.HttpContext.Response.Headers.SetCookie.ToString();
        Assert.StartsWith(ShowConsentAgainOnExpiredFormAttribute.CookieName + "=1", cookie);
        Assert.Contains("path=/connect/authorize", cookie);
        Assert.Contains("httponly", cookie);
    }

    [Fact]
    public async Task RefusedToken_TheMarkIsAnEssentialCookie()
    {
        // The platform's cookie policy asks for consent, and drops a cookie that is not essential
        var context = CreateContext(new AntiforgeryValidationFailedResult());
        var cookies = new RecordingCookies();
        context.HttpContext.Features.Set<Microsoft.AspNetCore.Http.Features.IResponseCookiesFeature>(cookies);

        await _filter.OnResultExecutionAsync(context, () => Task.FromResult<ResultExecutedContext>(null));

        Assert.True(cookies.Options[ShowConsentAgainOnExpiredFormAttribute.CookieName].IsEssential);
    }

    private sealed class RecordingCookies : Microsoft.AspNetCore.Http.Features.IResponseCookiesFeature, IResponseCookies
    {
        public Dictionary<string, CookieOptions> Options { get; } = [];

        public IResponseCookies Cookies => this;

        public void Append(string key, string value) => Options[key] = new CookieOptions();

        public void Append(string key, string value, CookieOptions options) => Options[key] = options;

        public void Delete(string key) => Options.Remove(key);

        public void Delete(string key, CookieOptions options) => Options.Remove(key);
    }

    [Fact]
    public async Task AnyOtherResult_IsLeftAsItIs()
    {
        var result = new ForbidResult();
        var context = CreateContext(result);

        await _filter.OnResultExecutionAsync(context, () => Task.FromResult<ResultExecutedContext>(null));

        Assert.Same(result, context.Result);
        Assert.Equal(0, context.HttpContext.Response.Headers.SetCookie.Count);
    }

    private static ResultExecutingContext CreateContext(IActionResult result)
    {
        var httpContext = new DefaultHttpContext { RequestServices = new ServiceCollection().AddAntiforgery().BuildServiceProvider() };
        httpContext.Request.Method = "POST";
        httpContext.Request.Path = "/connect/authorize";
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>
        {
            ["client_id"] = "claude",
            ["scope"] = "openid profile",
            ["state"] = "opaque",
            ["__RequestVerificationToken"] = "refused",
            ["submit.Accept"] = "Allow",
        });

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        return new ResultExecutingContext(actionContext, [], result, controller: null);
    }

    private sealed class AntiforgeryValidationFailedResult : BadRequestResult, Microsoft.AspNetCore.Mvc.Core.Infrastructure.IAntiforgeryValidationFailedResult;
}
