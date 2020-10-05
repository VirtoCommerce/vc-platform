using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Hangfire.Extensions
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
                // Call UseSqlServerStorage with fake SqlServerStorageOptions to avoid Hangfire tries to apply its migrations because these never do in case of database absence.
                // Real options provided in ApplicationBuilderExtensions.UseHangfire where migrations forced to apply.
                .UseSqlServerStorage(configuration.GetConnectionString("VirtoCommerce"), new SqlServerStorageOptions() { PrepareSchemaIfNecessary = false }));
            }
            else
            {
                services.AddHangfire(config => config.UseMemoryStorage());
            }

            return services;
        }
    }
}
