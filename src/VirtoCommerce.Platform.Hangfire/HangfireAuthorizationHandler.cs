using Hangfire.Dashboard;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Hangfire
{
    public class HangfireAuthorizationHandler : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            var result = httpcontext != null && httpcontext.User.Identity.IsAuthenticated;
            if (result)
            {
                result = httpcontext.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator) || httpcontext.User.HasClaim(PlatformConstants.Security.Claims.PermissionClaimType, PlatformConstants.Security.Permissions.BackgroundJobsManage);
            }
            return result;
        }
    }
}
