using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSessionsService : IUserSessionsService
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;

    public UserSessionsService(IOpenIddictTokenManager tokenManager, IOpenIddictAuthorizationManager authorizationManager)
    {
        _tokenManager = tokenManager;
        _authorizationManager = authorizationManager;
    }

    public virtual async Task TerminateUserSession(string sessionId)
    {
        var tokenObject = await _tokenManager.FindByIdAsync(sessionId);
        if (tokenObject != null && tokenObject is VirtoOpenIddictEntityFrameworkCoreToken token)
        {
            var authorizationId = await _tokenManager.GetAuthorizationIdAsync(token);

            await _tokenManager.TryRevokeAsync(token);

            var validSessionsCount = await _tokenManager.FindByAuthorizationIdAsync(authorizationId)
                .OfType<VirtoOpenIddictEntityFrameworkCoreToken>()
                .Where(x => x.Type == "refresh_token")
                .Where(x => x.Status == "valid")
                .CountAsync();

            if (validSessionsCount == 0)
            {
                await TerminateUserSessionGroup(authorizationId);
            }
        }
    }

    public virtual async Task TerminateUserSessionGroup(string sessionGroupId)
    {
        var tokens = _tokenManager.FindByAuthorizationIdAsync(sessionGroupId);
        await foreach (var token in tokens)
        {
            await _tokenManager.TryRevokeAsync(token);
        }

        var authorization = await _authorizationManager.FindByIdAsync(sessionGroupId);
        if (authorization != null)
        {
            await _authorizationManager.TryRevokeAsync(authorization);
        }
    }

    public virtual async Task TerminateAllUserSessions(string userId)
    {
        var tokens = _tokenManager.FindBySubjectAsync(userId);
        await foreach (var token in tokens)
        {
            await _tokenManager.TryRevokeAsync(token);
        }

        var authorizations = _authorizationManager.FindBySubjectAsync(userId);
        await foreach (var authorization in authorizations)
        {
            await _authorizationManager.TryRevokeAsync(authorization);
        }
    }
}
