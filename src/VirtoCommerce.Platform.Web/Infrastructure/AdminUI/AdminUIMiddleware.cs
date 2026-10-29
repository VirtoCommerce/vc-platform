using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Infrastructure.AdminUI;

/// <summary>
/// Decides which admin UI answers a request, before static files and routing:
/// <list type="bullet">
/// <item>A request under <c>/apps/{appId}/</c> of an <see cref="AdminUIHosting.Endpoint"/> admin UI runs with that path as
/// its path base, so the UI's page and every URL relative to it work there as they do at the site root.</item>
/// <item><c>/</c> goes to the root admin UI: an <see cref="AdminUIHosting.App"/> one gets <c>/apps/{appId}/</c> (its index page,
/// the address stays <c>/</c>); for an <see cref="AdminUIHosting.Endpoint"/> one, or none, <see cref="AdminUIMatcherPolicy"/>
/// picks the page.</item>
/// </list>
/// </summary>
public class AdminUIMiddleware(RequestDelegate next, IAdminUIAppService adminUIAppService)
{
    public Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        foreach (var adminUI in adminUIAppService.GetAdminUIs())
        {
            if (adminUI.Hosting == AdminUIHosting.Endpoint &&
                request.Path.StartsWithSegments(adminUI.Path, out var matched, out var remaining))
            {
                if (!remaining.HasValue)
                {
                    // Relative URLs of the page resolve against /apps/{appId}/ only with the trailing slash
                    context.Response.Redirect($"{request.PathBase}{matched}/{request.QueryString}");
                    return Task.CompletedTask;
                }

                return InvokeWithPathBaseAsync(context, matched, remaining, adminUI.Id);
            }
        }

        if (request.Path == "/" || !request.Path.HasValue)
        {
            var root = adminUIAppService.GetRootAdminUI();

            if (root?.Hosting == AdminUIHosting.App)
            {
                request.Path = new PathString($"{root.Path}/");
            }
            else
            {
                context.Features.Set(new AdminUIFeature(root?.Id));
            }
        }

        return next(context);
    }

    private async Task InvokeWithPathBaseAsync(HttpContext context, PathString matched, PathString remaining, string appId)
    {
        var request = context.Request;
        var originalPath = request.Path;
        var originalPathBase = request.PathBase;

        request.PathBase = originalPathBase.Add(matched);
        request.Path = remaining;
        context.Features.Set(new AdminUIFeature(appId));

        try
        {
            await next(context);
        }
        finally
        {
            request.PathBase = originalPathBase;
            request.Path = originalPath;
        }
    }
}

/// <summary>
/// The admin UI chosen for the request; a null <see cref="AppId"/> means the platform's own page.
/// </summary>
public sealed class AdminUIFeature(string appId)
{
    public string AppId { get; } = appId;
}
