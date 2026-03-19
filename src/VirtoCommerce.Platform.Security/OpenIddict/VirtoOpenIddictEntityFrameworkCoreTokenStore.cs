using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenIddict.EntityFrameworkCore;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreTokenStore : OpenIddictEntityFrameworkCoreTokenStore<VirtoOpenIddictEntityFrameworkCoreToken,
     VirtoOpenIddictEntityFrameworkCoreApplication,
     VirtoOpenIddictEntityFrameworkCoreAuthorization,
     string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VirtoOpenIddictEntityFrameworkCoreTokenStore(
        IMemoryCache cache,
        IOpenIddictEntityFrameworkCoreContext context,
        IOptionsMonitor<OpenIddictEntityFrameworkCoreOptions> options,
        IHttpContextAccessor httpContextAccessor
        ) : base(cache, context, options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask CreateAsync(VirtoOpenIddictEntityFrameworkCoreToken token, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var request = httpContext.Request;
            SetIp(token, httpContext);
            SetUserAgent(token, request);
        }

        return base.CreateAsync(token, cancellationToken);
    }

    protected virtual void SetIp(VirtoOpenIddictEntityFrameworkCoreToken token, HttpContext httpContext)
    {
        token.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
    }

    protected virtual void SetUserAgent(VirtoOpenIddictEntityFrameworkCoreToken token, HttpRequest request)
    {
        token.UserAgent = request.Headers.UserAgent;
    }
}
