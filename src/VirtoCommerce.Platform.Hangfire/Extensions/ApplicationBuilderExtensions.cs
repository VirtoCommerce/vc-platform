using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;
using VirtoCommerce.Platform.Hangfire.Middleware;

namespace VirtoCommerce.Platform.Hangfire.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder appBuilder, IConfiguration configuration)
        {
            // Always resolve Hangfire.IGlobalConfiguration to enforce correct initialization of Hangfire giblets before modules init.
            // Don't remove next line, it will cause issues with modules startup at hangfire-less (UseHangfireServer=false) platform instances.
            var hangfireGlobalConfiguration = appBuilder.ApplicationServices.GetRequiredService<IGlobalConfiguration>();

            var databaseProvider = configuration.GetValue("DatabaseProvider", "SqlServer");

            // This is an important workaround of Hangfire initialization issues
            // The standard database schema initialization way described at the page https://docs.hangfire.io/en/latest/configuration/using-sql-server.html works on an existing database only.
            // Therefore we create SqlServerStorage for Hangfire manually here.
            // This way we ensure Hangfire schema will be applied to storage AFTER platform database creation.
            var hangfireOptions = appBuilder.ApplicationServices.GetRequiredService<IOptions<HangfireOptions>>().Value;
            if (hangfireOptions.JobStorageType == HangfireJobStorageType.SqlServer ||
                hangfireOptions.JobStorageType == HangfireJobStorageType.Database)
            {
                var connectionString = configuration.GetConnectionString("VirtoCommerce");

                JobStorage storage = null;

                switch (databaseProvider)
                {
                    case "PostgreSql":
                        storage = new PostgreSqlStorage(connectionString, hangfireOptions.PostgreSqlStorageOptions);
                        break;
                    case "MySql":
                        storage = new MySqlStorage(connectionString, hangfireOptions.MySqlStorageOptions);
                        break;
                    default:
                        storage = new SqlServerStorage(connectionString, hangfireOptions.SqlServerStorageOptions);
                        break;
                }

                hangfireGlobalConfiguration.UseStorage(storage);
                hangfireGlobalConfiguration.UseConsole();

            }

            appBuilder.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthorizationHandler() } });

            var mvcJsonOptions = appBuilder.ApplicationServices.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
            GlobalConfiguration.Configuration.UseSerializerSettings(mvcJsonOptions.Value.SerializerSettings);

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            var recurringJobManager = appBuilder.ApplicationServices.GetService<IRecurringJobManager>();
            var settingsManager = appBuilder.ApplicationServices.GetService<ISettingsManager>();
            inProcessBus.RegisterHandler<ObjectSettingChangedEvent>(async (message, token) => await recurringJobManager.HandleSettingChangeAsync(settingsManager, message));

            // Add Hangfire filters/middlewares
            var userNameResolver = appBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IUserNameResolver>();
            GlobalJobFilters.Filters.Add(new HangfireUserContextMiddleware(userNameResolver));

            return appBuilder;
        }
    }
}
