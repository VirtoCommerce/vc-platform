using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a handler for a custom OAuth grant type and enables that grant type in OpenIddict.
    /// </summary>
    public static IServiceCollection AddGrantTypeHandler<THandler>(this IServiceCollection services, string grantType)
        where THandler : class, IGrantTypeHandler
    {
        services.AddTransient<IGrantTypeHandler, THandler>();

        services
            .AddOpenIddict()
            .AddServer(serverBuilder => serverBuilder.AllowCustomFlow(grantType));

        return services;
    }
}
