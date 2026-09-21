using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSessionsSearchService : IUserSessionsSearchService
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IScopedServiceFactory<ISecurityRepository> _repositoryFactory;

    [Obsolete("Use the constructor that takes IScopedServiceFactory<T> instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public UserSessionsSearchService(IOpenIddictTokenManager tokenManager, Func<ISecurityRepository> repositoryFactory)
        : this(tokenManager, new DelegateScopedServiceFactory<ISecurityRepository>(repositoryFactory))
    {
    }

    [ActivatorUtilitiesConstructor]
    public UserSessionsSearchService(IOpenIddictTokenManager tokenManager, IScopedServiceFactory<ISecurityRepository> repositoryFactory)
    {
        _tokenManager = tokenManager;
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task<UserSessionSearchResult> SearchAsync(UserSessionSearchCriteria criteria, bool clone = true)
    {
        var result = AbstractTypeFactory<UserSessionSearchResult>.TryCreateInstance();

        var tokens = await _tokenManager.FindBySubjectAsync(criteria.UserId)
            .OfType<VirtoOpenIddictEntityFrameworkCoreToken>()
            .Where(x => x.Type == OpenIddictConstants.TokenTypeIdentifiers.RefreshToken)
            .Where(x => x.Status == OpenIddictConstants.Statuses.Valid)
            .OrderByDescending(x => x.CreationDate)
            .ToListAsync();

        result.TotalCount = tokens.Count;

        if (criteria.Take > 0)
        {
            var tokensPage = tokens
                .Skip(criteria.Skip)
                .Take(criteria.Take)
                .ToList();

            foreach (var token in tokensPage)
            {
                var userSession = AbstractTypeFactory<UserSession>.TryCreateInstance();

                userSession.Id = token.Id;
                userSession.SessionGroupId = await _tokenManager.GetAuthorizationIdAsync(token);
                userSession.CreatedDate = token.CreationDate ?? DateTime.MinValue;
                userSession.IpAddress = token.IpAddress;
                userSession.UserAgent = token.UserAgent;
                userSession.ExpirationDate = token.ExpirationDate ?? DateTime.MinValue;

                result.Results.Add(userSession);
            }

            await MarkImpersonatedSessions(result.Results);
        }

        return result;
    }

    /// <summary>
    /// Marks the sessions that were opened by the login-on-behalf grant, so the existing Terminate
    /// command reads as a kill switch for a live impersonation session.
    /// The token itself carries no readable principal, so this joins on the sign-in audit log through
    /// the OpenIddict authorization id — one query for the whole page, never per row.
    /// </summary>
    protected virtual async Task MarkImpersonatedSessions(IList<UserSession> sessions)
    {
        var sessionGroupIds = sessions
            .Select(x => x.SessionGroupId)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToArray();

        if (sessionGroupIds.Length == 0)
        {
            return;
        }

        using var scopedRepository = _repositoryFactory.Create();
        var repository = scopedRepository.Service;

        var impersonations = await repository.UserSignInLogs
            .Where(x => x.SignInType == SignInType.Impersonation && sessionGroupIds.Contains(x.SessionId))
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new { x.SessionId, x.OperatorUserId, x.OperatorUserName })
            .ToListAsync();

        if (impersonations.Count == 0)
        {
            return;
        }

        var bySession = impersonations
            .GroupBy(x => x.SessionId)
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var session in sessions)
        {
            if (session.SessionGroupId != null && bySession.TryGetValue(session.SessionGroupId, out var impersonation))
            {
                session.IsImpersonated = true;
                session.OperatorUserId = impersonation.OperatorUserId;
                session.OperatorUserName = impersonation.OperatorUserName;
            }
        }
    }
}
