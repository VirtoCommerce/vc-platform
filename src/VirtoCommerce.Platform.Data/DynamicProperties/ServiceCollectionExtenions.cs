using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Validators;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddDynamicProperties(this IServiceCollection services)
        {
            services.AddSingleton<IDynamicPropertyService, DynamicPropertyService>();
            services.AddSingleton<IDynamicPropertySearchService, DynamicPropertySearchService>();
            services.AddSingleton<IDynamicPropertyRegistrar, DynamicPropertyService>();
            services.AddSingleton<IDynamicPropertyDictionaryItemsSearchService, DynamicPropertyDictionaryItemsSearchService>();
            services.AddSingleton<IDynamicPropertyDictionaryItemsService, DynamicPropertyDictionaryItemsService>();
            services.AddSingleton<IDynamicPropertyMetaDataResolver, DynamicPropertyMetaDataResolver>();
            services.AddTransient<AbstractValidator<DynamicProperty>, DynamicPropertyTypeValidator>();

            return services;
        }
    }
}
