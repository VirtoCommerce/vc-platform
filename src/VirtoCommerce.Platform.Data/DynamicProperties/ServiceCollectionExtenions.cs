using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Validators;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddDynamicProperties(this IServiceCollection services)
        {
            services.AddActivated<IDynamicPropertyService, DynamicPropertyService>(ServiceLifetime.Singleton);
            services.AddActivated<IDynamicPropertySearchService, DynamicPropertySearchService>(ServiceLifetime.Singleton);
            services.AddActivated<IDynamicPropertyRegistrar, DynamicPropertyService>(ServiceLifetime.Singleton);
            services.AddActivated<IDynamicPropertyDictionaryItemsSearchService, DynamicPropertyDictionaryItemsSearchService>(ServiceLifetime.Singleton);
            services.AddActivated<IDynamicPropertyDictionaryItemsService, DynamicPropertyDictionaryItemsService>(ServiceLifetime.Singleton);
            services.AddSingleton<IDynamicPropertyMetaDataResolver, DynamicPropertyMetaDataResolver>();
            services.AddSingleton<AbstractValidator<DynamicProperty>, DynamicPropertyTypeValidator>();

            return services;
        }
    }
}
