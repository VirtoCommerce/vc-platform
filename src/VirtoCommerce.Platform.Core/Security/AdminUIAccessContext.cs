using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Everything an <see cref="IAdminUIAccessPolicy"/> needs to reach a decision.
    /// Passing a context instead of loose parameters keeps the interface stable:
    /// new inputs can be added as properties without breaking existing implementations,
    /// which matters because Platform.Core ships as a NuGet package.
    /// </summary>
    public class AdminUIAccessContext
    {
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Distinct permission names the user holds, flattened from their roles.
        /// </summary>
        public IList<string> Permissions { get; set; } = new List<string>();
    }
}
