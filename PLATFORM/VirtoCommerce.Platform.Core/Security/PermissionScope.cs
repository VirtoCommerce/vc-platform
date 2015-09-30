using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Base class for all types repesents permision boundary scopes
    /// </summary>
    public abstract class PermissionScope : ValueObject<PermissionScope>
    {
        public PermissionScope()
        {
            Type = this.GetType().Name;
        }
        /// <summary>
        /// Define scope type influences the choice of ui pattern in role definition 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Represent string scope value
        /// </summary>
        public string Scope { get; set; }
    }
}
