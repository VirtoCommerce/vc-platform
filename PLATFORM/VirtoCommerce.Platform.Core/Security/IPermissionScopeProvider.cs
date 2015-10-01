using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IPermissionScopeProvider
    {
        /// <summary>
        /// Return all supported scope for given permission used in role permission configuration 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        IEnumerable<PermissionScope> GetPermissionScopes(string permission);
        /// <summary>
        /// Return resulting list of scope string for entity may be used for permissions check
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IEnumerable<string> GetEntityScopes(Entity entity);
    }
}
