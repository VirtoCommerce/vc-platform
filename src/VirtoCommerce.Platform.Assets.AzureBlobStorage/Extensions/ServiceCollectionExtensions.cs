using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Assets;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAzureBlobProvider(this IServiceCollection services, Action<AzureBlobOptions> setupAction = null)
        {
            services.AddSingleton<IBlobStorageProvider, AzureBlobProvider>();
            services.AddSingleton<IBlobUrlResolver, AzureBlobProvider>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
