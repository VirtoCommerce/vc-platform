using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VirtoCommerce.Platform.Web.Extensions;

public static class StaticFileResponseContextExtensions
{
    public static void SetCacheControlNoCache(this StaticFileResponseContext context)
    {
        if (context.Context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            context.Context.Response.Headers.CacheControl = "no-cache";
        }
    }
}
