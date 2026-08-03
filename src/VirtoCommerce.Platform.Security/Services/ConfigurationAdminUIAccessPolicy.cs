using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services
{
    /// <summary>
    /// Evaluates admin UI access against the VirtoCommerce:PlatformUI:Access configuration section.
    /// Keeping the policy in configuration rather than in a stored setting means it cannot be
    /// changed through the API, so a user cannot grant themselves admin UI access.
    /// </summary>
    public class ConfigurationAdminUIAccessPolicy : IAdminUIAccessPolicy
    {
        // A monitor rather than IOptions so that editing appsettings.json takes effect
        // without restarting the process - this class is registered as a singleton.
        private readonly IOptionsMonitor<PlatformUIOptions> _platformUIOptions;

        public ConfigurationAdminUIAccessPolicy(IOptionsMonitor<PlatformUIOptions> platformUIOptions)
        {
            _platformUIOptions = platformUIOptions;
        }

        public Task<AdminUIAccessResult> EvaluateAsync(AdminUIAccessContext context, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(context.User);

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(Evaluate(context));
        }

        private AdminUIAccessResult Evaluate(AdminUIAccessContext context)
        {
            var options = _platformUIOptions.CurrentValue?.Access ?? new AdminUIAccessOptions();
            var user = context.User;
            var permissions = context.Permissions ?? [];

            // 1. Administrators bypass every other rule.
            if (options.AllowAdministrators && user.IsAdministrator)
            {
                return AdminUIAccessResult.Allowed();
            }

            // 2. Account type is an allow-list: an empty list does not constrain.
            if (options.AllowedAccountTypes?.Count > 0 &&
                !options.AllowedAccountTypes.Any(x => x.EqualsIgnoreCase(user.UserType)))
            {
                return AdminUIAccessResult.Denied($"Account type '{user.UserType}' is not in AllowedAccountTypes.");
            }

            // 3. Denied permission masks win over any allow rule.
            var deniedMatch = FirstMatch(options.DeniedPermissions, permissions);
            if (deniedMatch != null)
            {
                return AdminUIAccessResult.Denied($"Permission '{deniedMatch}' matches DeniedPermissions.");
            }

            // 4. When an allow-list is configured the user must hold at least one matching permission.
            if (options.AllowedPermissions?.Count > 0 && FirstMatch(options.AllowedPermissions, permissions) == null)
            {
                return AdminUIAccessResult.Denied("No permission matches AllowedPermissions.");
            }

            // 5. The platform's original rule: at least one permission is required.
            if (options.RequireAnyPermission && permissions.Count == 0)
            {
                return AdminUIAccessResult.Denied("The user has no permissions.");
            }

            return AdminUIAccessResult.Allowed();
        }

        /// <summary>
        /// Returns the first permission matching any of the masks, or null when none match.
        /// </summary>
        private static string FirstMatch(IList<string> masks, IList<string> permissions)
        {
            if (masks is null || masks.Count == 0)
            {
                return null;
            }

            return permissions.FirstOrDefault(permission => masks.Any(mask => WildcardMatcher.IsMatch(mask, permission)));
        }
    }
}
