using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IHasSecurityAccounts : IEntity
    {
        /// <summary>
        /// All security accounts 
        /// </summary>
        ICollection<ApplicationUser> SecurityAccounts { get; set; }
    }
}
