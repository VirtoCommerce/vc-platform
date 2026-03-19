using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Data.Localizations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizedItems(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILocalizedItemService, LocalizedItemService>();
            serviceCollection.AddSingleton<ILocalizedItemSearchService, LocalizedItemSearchService>();

            return serviceCollection;
        }
    }
}
