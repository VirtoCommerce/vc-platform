using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Platform.Core.Security
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Permission[] Permissions { get; set; }
        public RoleScope[] Scopes { get; set; }

        /// <summary>
        /// Generate permission:scope type:scope string will used in check permissions by string
        /// </summary>
        public IEnumerable<string> GetPermissonFullQualifiedNames()
        {

            if (Permissions != null)
            {
                if (Permissions != null)
                {
                    var permissionStr = Permissions.Select(x => x.Id).ToArray();
                    var sopesStr = Scopes != null ? Scopes.Select(x => String.Format("{0}:{1}", x.Type, x.Scope)).ToArray() : null;
                    return permissionStr.LeftJoin(sopesStr, ":");
                }
            }
            return Enumerable.Empty<string>();
        }

    }
}
