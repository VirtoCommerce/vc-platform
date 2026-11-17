using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Validation.OpenIddictValidationEvents;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// The platform doesn't check token audiences, so the token of a self-registered client is accepted only on the path
/// of a resource it was issued for, not by the rest of the platform's API.
/// </summary>
public class ValidateSelfRegisteredClientTokenAudience : IOpenIddictValidationHandler<ProcessAuthenticationContext>
{
    private const int OrderAfterAccessTokenValidation = 1000;

    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValidateSelfRegisteredClientTokenAudience(IOpenIddictApplicationManager applicationManager, IHttpContextAccessor httpContextAccessor)
    {
        _applicationManager = applicationManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public static OpenIddictValidationHandlerDescriptor Descriptor { get; } = OpenIddictValidationHandlerDescriptor
        .CreateBuilder<ProcessAuthenticationContext>()
        .UseScopedHandler<ValidateSelfRegisteredClientTokenAudience>()
        .SetOrder(OpenIddictValidationHandlers.ValidateAccessToken.Descriptor.Order + OrderAfterAccessTokenValidation)
        .SetType(OpenIddictValidationHandlerType.Custom)
        .Build();

    public virtual async ValueTask HandleAsync(ProcessAuthenticationContext context)
    {
        var resources = (context.AccessTokenPrincipal?.GetAudiences() ?? [])
            .Select(x => Uri.TryCreate(x, UriKind.Absolute, out var uri) ? uri : null)
            .Where(x => x != null)
            .ToList();

        var clientId = context.AccessTokenPrincipal?.GetClaim(Claims.ClientId);

        if (resources.Count == 0 || string.IsNullOrEmpty(clientId))
        {
            return;
        }

        var application = await FindStoredByClientIdAsync(clientId);
        if (application == null)
        {
            context.Reject(Errors.InvalidToken, "The client application of the access token is not available.");
            return;
        }

        if (await _applicationManager.IsSelfRegisteredAsync(application) && !resources.Any(IsRequestToResource))
        {
            context.Reject(Errors.InvalidToken, "The access token was issued for another resource.");
        }
    }

    protected virtual async Task<object> FindStoredByClientIdAsync(string clientId)
    {
        return _applicationManager is VirtoOpenIddictApplicationManager manager
            ? await manager.FindStoredByClientIdAsync(clientId)
            : await _applicationManager.FindByClientIdAsync(clientId);
    }

    protected virtual bool IsRequestToResource(Uri resource)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
        {
            return false;
        }

        var path = (request.PathBase + request.Path).Value?.TrimEnd('/') ?? string.Empty;
        var resourcePath = resource.AbsolutePath.TrimEnd('/');
        if (resourcePath.Length == 0)
        {
            return false;
        }

        return path.Equals(resourcePath, StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith(resourcePath + "/", StringComparison.OrdinalIgnoreCase);
    }
}
