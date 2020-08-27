using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Hangfire
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("VirtoCommerce:Hangfire");
            var hangfireOptions = new HangfireOptions();
            section.Bind(hangfireOptions);
            services.AddOptions<HangfireOptions>().Bind(section).ValidateDataAnnotations();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = hangfireOptions.AutomaticRetryCount });
            if (hangfireOptions.JobStorageType == HangfireJobStorageType.SqlServer)
            {
                services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("VirtoCommerce"), hangfireOptions.SqlServerStorageOptions));
            }
            else
            {
                services.AddHangfire(config => config.UseMemoryStorage());
            }

            return services;
        }
    }
}
