using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Represent role assignment bounding scope.
    /// Ex: Specified catalog or store etc.
    /// </summary>
    public class RoleScope : ValueObject<RoleScope>
    {
        /// <summary>
        /// Scope identifier (id for store or another bounding object)
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// Scope type (Store, Catalog, Site etc)
        /// </summary>
        public string Type { get; set; }
    }
}
