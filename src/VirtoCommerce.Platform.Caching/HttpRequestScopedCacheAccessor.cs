using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Caching;

/// <summary>
/// <see cref="IRequestScopedCacheAccessor"/> backed by <see cref="IHttpContextAccessor"/>.
/// </summary>
/// <remarks>
/// Safe to register as a singleton: it holds only <see cref="IHttpContextAccessor"/>, which reads the ambient
/// context from an <c>AsyncLocal</c> rather than capturing one. Resolving through
/// <see cref="HttpContext.RequestServices"/> - the live request's own scope - is what makes the returned cache
/// correct no matter which scope constructed the consumer; deferring the resolution alone would not, because a
/// captured <c>IServiceProvider</c> still belongs to the scope it was captured from.
/// </remarks>
public class HttpRequestScopedCacheAccessor(IHttpContextAccessor httpContextAccessor) : IRequestScopedCacheAccessor
{
    /// <inheritdoc />
    /// <remarks>
    /// Both null-conditionals are load-bearing: <see cref="IHttpContextAccessor.HttpContext"/> is null outside a
    /// request, and <see cref="HttpContext.RequestServices"/> is null on a fabricated <see cref="DefaultHttpContext"/>.
    /// <see cref="ServiceProviderServiceExtensions.GetService{T}"/> is used rather than its required counterpart so
    /// a host that did not call <c>AddCaching</c> yields null instead of throwing. Nothing is memoized: the context
    /// is re-read on every access, which is what makes the result follow the ambient request.
    /// </remarks>
    public virtual IRequestScopedCache Current =>
        httpContextAccessor.HttpContext?.RequestServices?.GetService<IRequestScopedCache>();
}
