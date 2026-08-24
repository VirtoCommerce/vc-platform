using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
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

            return appBuilder;
        }

        public static IApplicationBuilder UseAccountLockoutMiddleware(this IApplicationBuilder appBuilder, string identityCookieName)
        {
            appBuilder.Use(async (context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true &&
                    context.Request.Cookies.ContainsKey(identityCookieName))
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
