using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Caching;

/// <summary>
/// <see cref="IRequestScopedCacheAccessor"/> backed by <see cref="IHttpContextAccessor"/>.
/// </summary>
public class HttpRequestScopedCacheAccessor(IHttpContextAccessor httpContextAccessor) : IRequestScopedCacheAccessor
{
    // Safe as a singleton: the only captured dependency is IHttpContextAccessor, whose AsyncLocal holder is
    // static, so no per-request state is held here.
    // HttpContext is null outside a request; RequestServices is null on a fabricated DefaultHttpContext.
    // GetService, not GetRequiredService: a host that never called AddCaching should get null, not an exception.
    public virtual IRequestScopedCache Cache =>
        httpContextAccessor.HttpContext?.RequestServices?.GetService<IRequestScopedCache>();
}
