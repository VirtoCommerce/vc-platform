using System;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSessionsSearchService : IUserSessionsSearchService
{
    private readonly IOpenIddictTokenManager _tokenManager;

    public UserSessionsSearchService(IOpenIddictTokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }

    public virtual async Task<UserSessionSearchResult> SearchAsync(UserSessionSearchCriteria criteria, bool clone = true)
    {
        var result = AbstractTypeFactory<UserSessionSearchResult>.TryCreateInstance();

        var tokens = await _tokenManager.FindBySubjectAsync(criteria.UserId)
            .OfType<VirtoOpenIddictEntityFrameworkCoreToken>()
            .Where(x => x.Type == "refresh_token")
            .Where(x => x.Status == "valid")
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

                userSession.Id = await _tokenManager.GetAuthorizationIdAsync(token);
                userSession.CreatedDate = token.CreationDate ?? DateTime.MinValue;
                userSession.IpAddress = token.IpAddress;
                userSession.UserAgent = token.UserAgent;
                userSession.ExpirationDate = token.ExpirationDate ?? DateTime.MinValue;

                result.Results.Add(userSession);
            }
        }

        return result;
    }
}
