using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Assets;

namespace VirtoCommerce.Platform.Assets.FileSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileSystemBlobProvider(this IServiceCollection services, Action<FileSystemBlobOptions> setupAction = null)
        {
            services.AddSingleton<IBlobStorageProvider, FileSystemBlobProvider>();
            services.AddSingleton<IBlobUrlResolver, FileSystemBlobProvider>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
        }
    }
}
