using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Jobs;

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
                Authorization = authorizationFilters
            };

            app.UseHangfireDashboard(appPath + "hangfire", dashboardOptions);

            // Configure Hangfire server
            if (_options.StartServer)
            {
                var serverOptions = new BackgroundJobServerOptions
                {
                    Activator = new UnityJobActivator(container),

                    // Create some queues for job prioritization.
                    // Normal equals 'default', because Hangfire depends on it.
                    Queues = new[] {JobPriority.High, JobPriority.Normal, JobPriority.Low}
                };

                // Allow tweaking worker thread count.
                if (_options.WorkerCount.HasValue)
                    serverOptions.WorkerCount = _options.WorkerCount.Value;

                app.UseHangfireServer(serverOptions);
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

                result = new SqlServerStorage(_options.DatabaseConnectionString, sqlServerStorageOptions);
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
