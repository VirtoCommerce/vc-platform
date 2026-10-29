using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Infrastructure.AdminUI;

/// <summary>
/// Several admin UIs, and the platform's own "no admin UI" page, map <c>/</c> (<see cref="AdminUIPageAttribute"/>).
/// Keeps the page of the admin UI that <see cref="AdminUIMiddleware"/> chose for the request, or the platform's page
/// when that admin UI has none.
/// </summary>
public sealed class AdminUIMatcherPolicy(IAdminUIAppService adminUIAppService) : MatcherPolicy, IEndpointSelectorPolicy
{
    public override int Order => 0;

    public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
    {
        return endpoints.Any(x => x.Metadata.GetMetadata<AdminUIPageAttribute>() != null);
    }

    public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
    {
        var appId = httpContext.Features.Get<AdminUIFeature>() is { } feature
            ? feature.AppId
            : adminUIAppService.GetRootAdminUI()?.Id;

        var platformPage = -1;
        var found = false;

        for (var i = 0; i < candidates.Count; i++)
        {
            var page = candidates.IsValidCandidate(i) ? candidates[i].Endpoint.Metadata.GetMetadata<AdminUIPageAttribute>() : null;

            if (page == null)
            {
                continue;
            }

            if (page.AppId == null)
            {
                platformPage = i;
            }
            else if (appId != null && page.AppId.EqualsIgnoreCase(appId))
            {
                found = true;
            }
            else
            {
                candidates.SetValidity(i, false);
            }
        }

        if (found && platformPage >= 0)
        {
            candidates.SetValidity(platformPage, false);
        }

        return Task.CompletedTask;
    }
}
