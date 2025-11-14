using System;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public class VirtoOpenIddictEntityFrameworkCoreTokenStoreResolver : IOpenIddictTokenStoreResolver
{
    private readonly IServiceProvider _provider;

    public VirtoOpenIddictEntityFrameworkCoreTokenStoreResolver(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IOpenIddictTokenStore<TToken> Get<TToken>() where TToken : class
    {
        var store = _provider.GetService<IOpenIddictTokenStore<TToken>>();
        if (store is not null)
        {
            return store;
        }

        var typeKey = typeof(TToken);

        if (typeKey == typeof(VirtoOpenIddictEntityFrameworkCoreToken))
        {
            var service = _provider.GetRequiredService<VirtoOpenIddictEntityFrameworkCoreTokenStore>();
            return (IOpenIddictTokenStore<TToken>)service;
        }

        throw new InvalidOperationException("Token type is not VirtoOpenIddictEntityFrameworkCoreToken.");
    }
}
