using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    public class HangfireLauncher
    {
        private readonly HangfireOptions _options;

        public HangfireLauncher(HangfireOptions options)
        {
            _options = options;
        }

        public void ConfigureDatabase()
        {
            CreateJobStorage(Stage.ConfigureDatabase);
        }

        public void ConfigureOwin(IAppBuilder app, IUnityContainer container)
        {
            JobStorage.Current = CreateJobStorage(Stage.ConfigureOwin);

            // Configure Hangfire dashboard

            var securityService = container.Resolve<ISecurityService>();
            var moduleInitializerOptions = container.Resolve<IModuleInitializerOptions>();

            var appPath = "/" + moduleInitializerOptions.RoutePrefix;

            var authorizationFilters = new[]
            {
                    new PermissionBasedAuthorizationFilter(securityService)
                    {
                        Permission = PredefinedPermissions.BackgroundJobsManage
                    }
                };

            var dashboardOptions = new DashboardOptions
            {
                AppPath = appPath,
                AuthorizationFilters = authorizationFilters
            };

            app.UseHangfireDashboard(appPath + "hangfire", dashboardOptions);

            // Configure Hangfire server
            if (_options.StartServer)
            {
                app.UseHangfireServer(new BackgroundJobServerOptions { Activator = new UnityJobActivator(container) });
            }
        }


        private JobStorage CreateJobStorage(Stage stage)
        {
            JobStorage result = null;

            if (string.Equals(_options.JobStorageType, "SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                var sqlServerStorageOptions = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = stage == Stage.ConfigureDatabase,
                    QueuePollInterval = TimeSpan.FromSeconds(60) /* Default is 15 seconds */
                };

                result = new SqlServerStorage(_options.DatabaseConnectionStringName, sqlServerStorageOptions);
            }
            else if (string.Equals(_options.JobStorageType, "Memory", StringComparison.OrdinalIgnoreCase))
            {
                result = new MemoryStorage();
            }

            return result;
        }

        private enum Stage
        {
            ConfigureDatabase,
            ConfigureOwin
        }
    }
}
