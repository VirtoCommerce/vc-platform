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
        public string[] Scopes { get; set; }

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
                    return permissionStr.LeftJoin(Scopes, ":");
                }
            }
            return Enumerable.Empty<string>();
        }

    }
}
