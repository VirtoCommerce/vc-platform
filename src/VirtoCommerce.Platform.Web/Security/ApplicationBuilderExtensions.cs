using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Hangfire;
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
            appBuilder.RegisterEventHandler<UserChangedEvent, UserApiKeyActualizeEventHandler>();

            appBuilder.RegisterEventHandler<UserChangedEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserPasswordChangedEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserResetPasswordEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserChangedPasswordEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserLoginEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserLogoutEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserRoleAddedEvent, LogChangesUserChangedEventHandler>();
            appBuilder.RegisterEventHandler<UserRoleRemovedEvent, LogChangesUserChangedEventHandler>();

            appBuilder.RegisterEventHandler<UserChangedEvent, RevokeUserTokenEventHandler>();
            appBuilder.RegisterEventHandler<UserLogoutEvent, RevokeUserTokenEventHandler>();

            return appBuilder;
        }

        /// <summary>
        /// Schedule a periodic job for prune expired/invalid authorization tokens
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePruneExpiredTokensJob(this IApplicationBuilder appBuilder)
        {
            var recurringJobService = appBuilder.ApplicationServices.GetService<IRecurringJobService>();

            recurringJobService.WatchJobSetting(
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

        public static IApplicationBuilder UseAccountLockoutMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = await userManager.GetUserAsync(context.User);

                    if (user != null && await userManager.IsLockedOutAsync(user))
                    {
                        await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }

                await next();
            });

            return appBuilder;
        }
    }
}
