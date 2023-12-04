using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Hangfire;
using VirtoCommerce.Platform.Hangfire.Extensions;
using VirtoCommerce.Platform.Security.Handlers;
using VirtoCommerce.Platform.Web.Security.BackgroundJobs;

namespace VirtoCommerce.Platform.Web.Security
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePlatformPermissions(this IApplicationBuilder appBuilder)
        {
            //Register PermissionScope type itself to prevent Json serialization error due that fact that will be taken from other derived from PermissionScope type (first in registered types list) in PolymorphJsonContractResolver
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScope>();

            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(PlatformConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "Platform", Name = x }).ToArray());
            return appBuilder;
        }

        public static IApplicationBuilder UseSecurityHandlers(this IApplicationBuilder appBuilder)
        {
            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<UserChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<UserApiKeyActualizeEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserPasswordChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserResetPasswordEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserLoginEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserLogoutEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserRoleAddedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserRoleRemovedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesUserChangedEventHandler>().Handle(message));

            return appBuilder;
        }

        /// <summary>
        /// Schedule a periodic job for prune expired/invalid authorization tokens
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePruneExpiredTokensJob(this IApplicationBuilder appBuilder)
        {
            var recurringJobManager = appBuilder.ApplicationServices.GetService<IRecurringJobManager>();
            var settingsManager = appBuilder.ApplicationServices.GetService<ISettingsManager>();

            recurringJobManager.WatchJobSetting(
                settingsManager,
                new SettingCronJobBuilder()
                    .SetEnablerSetting(PlatformConstants.Settings.Security.EnablePruneExpiredTokensJob)
                    .SetCronSetting(PlatformConstants.Settings.Security.CronPruneExpiredTokensJob)
                    .ToJob<PruneExpiredTokensJob>(x => x.Process())
                    .Build());

            return appBuilder;
        }

        /// <summary>
        /// Schedule a periodic job to lock out accounts whose last login date is older than the configured one
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="lockoutOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoAccountsLockoutJob(this IApplicationBuilder appBuilder, LockoutOptionsExtended lockoutOptions)
        {
            if (lockoutOptions.AutoAccountsLockoutJobEnabled)
            {
                RecurringJob.AddOrUpdate<AutoAccountLockoutJob>("AutoAccountLockoutJob", j => j.Process(), lockoutOptions.CronAutoAccountsLockoutJob);
            }
            else
            {
                RecurringJob.RemoveIfExists("AutoAccountLockoutJob");
            }

            return appBuilder;
        }
    }
}
