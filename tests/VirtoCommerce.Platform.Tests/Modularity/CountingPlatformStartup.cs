using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class CountingPlatformStartup : IPlatformStartup
{
    public int AfterRoutingCalls { get; private set; }

    public int AfterAuthenticationCalls { get; private set; }

    public void ConfigureAfterRouting(IApplicationBuilder app, IConfiguration config)
    {
        AfterRoutingCalls++;
    }

    public void ConfigureAfterAuthentication(IApplicationBuilder app, IConfiguration config)
    {
        AfterAuthenticationCalls++;
    }
}
