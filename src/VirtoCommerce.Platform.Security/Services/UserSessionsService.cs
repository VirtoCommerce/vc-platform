using System;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSessionsService : IUserSessionsService
{
    private readonly Func<IOpenIddictTokenManager> _tokenManagerFactory;
    private readonly Func<IOpenIddictAuthorizationManager> _authorizationManagerFactory;

    public UserSessionsService(Func<IOpenIddictTokenManager> tokenManagerFactory, Func<IOpenIddictAuthorizationManager> authorizationManagerFactory)
    {
        _tokenManagerFactory = tokenManagerFactory;
        _authorizationManagerFactory = authorizationManagerFactory;
    }

    public virtual async Task TerminateUserSession(string sessionId)
    {
        var tokenManager = _tokenManagerFactory();

        var tokenObject = await tokenManager.FindByIdAsync(sessionId);
        if (tokenObject is VirtoOpenIddictEntityFrameworkCoreToken token)
        {
            var authorizationId = await tokenManager.GetAuthorizationIdAsync(token);

            await tokenManager.TryRevokeAsync(token);

            if (authorizationId != null)
            {
                var validSessionsCount = await tokenManager.FindByAuthorizationIdAsync(authorizationId)
                    .OfType<VirtoOpenIddictEntityFrameworkCoreToken>()
                    .Where(x => x.Type == OpenIddictConstants.TokenTypeIdentifiers.RefreshToken)
                    .Where(x => x.Status == OpenIddictConstants.Statuses.Valid)
                    .CountAsync();

                if (validSessionsCount == 0)
                {
                    await TerminateUserSessionGroup(authorizationId);
                }
            }
        }
    }

    public virtual async Task TerminateUserSessionGroup(string sessionGroupId)
    {
        var tokenManager = _tokenManagerFactory();
        var authorizationManager = _authorizationManagerFactory();

        var tokens = tokenManager.FindByAuthorizationIdAsync(sessionGroupId);
        await foreach (var token in tokens)
        {
            await tokenManager.TryRevokeAsync(token);
        }

        var authorization = await authorizationManager.FindByIdAsync(sessionGroupId);
        if (authorization != null)
        {
            await authorizationManager.TryRevokeAsync(authorization);
        }
    }

    public virtual async Task TerminateAllUserSessions(string userId)
    {
        var tokenManager = _tokenManagerFactory();
        var authorizationManager = _authorizationManagerFactory();

        var tokens = tokenManager.FindBySubjectAsync(userId);
        await foreach (var token in tokens)
        {
            await tokenManager.TryRevokeAsync(token);
        }

        var authorizations = authorizationManager.FindBySubjectAsync(userId);
        await foreach (var authorization in authorizations)
        {
            await authorizationManager.TryRevokeAsync(authorization);
        }
    }
}
