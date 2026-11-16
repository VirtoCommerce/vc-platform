using System;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// A self-registered client must name the resource, so that its token works there only, and use PKCE with S256.
/// </summary>
public class ValidateSelfRegisteredClientAuthorizationRequest : IOpenIddictServerHandler<ValidateAuthorizationRequestContext>
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    public ValidateSelfRegisteredClientAuthorizationRequest(IOpenIddictApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; } = OpenIddictServerHandlerDescriptor
        .CreateBuilder<ValidateAuthorizationRequestContext>()
        .UseScopedHandler<ValidateSelfRegisteredClientAuthorizationRequest>()
        .SetOrder(OpenIddictServerHandlers.Authentication.ValidateProofKeyForCodeExchangeRequirement.Descriptor.Order + 500)
        .SetType(OpenIddictServerHandlerType.Custom)
        .Build();

    public virtual async ValueTask HandleAsync(ValidateAuthorizationRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var application = await _applicationManager.FindByClientIdAsync(context.ClientId);
        if (!await _applicationManager.IsSelfRegisteredAsync(application))
        {
            return;
        }

        if (!context.Request.GetResources().Any())
        {
            context.Reject(Errors.InvalidTarget, "The resource parameter is required: name the resource the token is for.");
            return;
        }

        if (context.Request.CodeChallengeMethod != CodeChallengeMethods.Sha256)
        {
            context.Reject(Errors.InvalidRequest, $"The code_challenge_method must be '{CodeChallengeMethods.Sha256}'.");
        }
    }
}
