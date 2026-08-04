using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Controls which authenticated users are allowed to enter the admin UI.
    /// Configured in the VirtoCommerce:PlatformUI:Access section.
    /// An empty list means "not constrained by this rule", so the defaults below
    /// reproduce the behavior the platform had before this option existed:
    /// any user holding at least one permission may enter the admin UI.
    /// </summary>
    public class AdminUIAccessOptions
    {
        /// <summary>
        /// When true (default), users with IsAdministrator bypass all other rules.
        /// </summary>
        public bool AllowAdministrators { get; set; } = true;

        /// <summary>
        /// When true (default), a user must hold at least one permission to enter the admin UI.
        /// </summary>
        public bool RequireAnyPermission { get; set; } = true;

        /// <summary>
        /// Account types (<see cref="UserType"/>) allowed to enter the admin UI.
        /// Allow-list only: when empty the account type is not checked.
        /// </summary>
        public IList<string> AllowedAccountTypes { get; set; } = new List<string>();

        /// <summary>
        /// Permission masks ('*' wildcard) of which the user must hold at least one.
        /// When empty the user's permissions are not required to match anything.
        /// </summary>
        public IList<string> AllowedPermissions { get; set; } = new List<string>();

        /// <summary>
        /// Permission masks ('*' wildcard) that deny admin UI access.
        /// Holding any matching permission denies access, and takes precedence
        /// over <see cref="AllowedPermissions"/>.
        /// </summary>
        public IList<string> DeniedPermissions { get; set; } = new List<string>();
    }
}
