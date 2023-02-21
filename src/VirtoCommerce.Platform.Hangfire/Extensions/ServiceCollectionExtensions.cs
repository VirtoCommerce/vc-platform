using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Hangfire.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IGlobalConfiguration AddHangfireStorage(this IGlobalConfiguration globalConfiguration, IConfiguration configuration)
        {
            var databaseProvider = configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = configuration.GetConnectionString("VirtoCommerce.Hangfire") ?? configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "PostgreSql":
                    globalConfiguration.UsePostgreSqlStorage(connectionString);
                    break;
                case "MySql":
                    globalConfiguration.UseStorage(new MySqlStorage(connectionString,
                        new MySqlStorageOptions { PrepareSchemaIfNecessary = false }));
                    break;
                default:
                    // Call UseSqlServerStorage with fake SqlServerStorageOptions to avoid Hangfire tries to apply its migrations because these never do in case of database absence.
                    // Real options provided in ApplicationBuilderExtensions.UseHangfire where migrations forced to apply.
                    globalConfiguration.UseSqlServerStorage(connectionString,
                        new SqlServerStorageOptions { PrepareSchemaIfNecessary = false });
                    break;
            }

            return globalConfiguration;
        }

        public static object AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("VirtoCommerce:Hangfire");
            var hangfireOptions = new HangfireOptions();
            section.Bind(hangfireOptions);
            services.AddOptions<HangfireOptions>().Bind(section).ValidateDataAnnotations();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = hangfireOptions.AutomaticRetryCount });

            if (hangfireOptions.JobStorageType == HangfireJobStorageType.SqlServer ||
                hangfireOptions.JobStorageType == HangfireJobStorageType.Database )
            {
                services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .AddHangfireStorage(configuration));
            }
            else
            {
                services.AddHangfire(config => config.UseMemoryStorage());
            }

            return services;
        }
    }
}
