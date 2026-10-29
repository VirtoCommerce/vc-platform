using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Infrastructure.AdminUI;

/// <summary>
/// Decides which admin UI answers a request, before static files and routing:
/// <list type="bullet">
/// <item>A request under <c>/apps/{appId}/</c> of an <see cref="AdminUIHosting.Endpoint"/> admin UI is handled as if it came
/// to the site root (<c>/apps/platform/api/...</c> as <c>/api/...</c>), so the UI's page and every URL relative to it work
/// there too. The path base is left alone, so cookies, token issuers and generated URLs stay those of the site root;
/// <see cref="AdminUIRequestFeature.MountPath"/> tells the page where it is.</item>
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
                request.Path.StartsWithSegments(adminUI.Path, out _, out var remaining))
            {
                if (!remaining.HasValue)
                {
                    // Relative URLs of the page resolve against /apps/{appId}/ only with the trailing slash.
                    // The Location is relative ("platform/", resolved against /apps/platform) and built from the
                    // manifest's app id only: nothing from the request (path casing, query, forwarded prefix) goes in it.
                    context.Response.Redirect($"{adminUI.Id}/");
                    return Task.CompletedTask;
                }

                return InvokeMountedAsync(context, remaining, adminUI.Id, adminUI.Path);
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
                context.Features.Set(new AdminUIRequestFeature(root?.Id, mountPath: null));
            }
        }

        return next(context);
    }

    private async Task InvokeMountedAsync(HttpContext context, PathString remaining, string appId, string mountPath)
    {
        var request = context.Request;
        var originalPath = request.Path;

        request.Path = remaining;
        context.Features.Set(new AdminUIRequestFeature(appId, mountPath));

        try
        {
            await next(context);
        }
        finally
        {
            request.Path = originalPath;
        }
    }
}
