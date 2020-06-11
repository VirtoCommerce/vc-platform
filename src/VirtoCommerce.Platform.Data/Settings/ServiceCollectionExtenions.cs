using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsManager, SettingsManager>();
            services.AddSingleton<ISettingsRegistrar>(context => context.GetService<ISettingsManager>());
            services.AddSingleton<ISettingsSearchService, SettingsSearchService>();

            return services;
        }
    }
}
