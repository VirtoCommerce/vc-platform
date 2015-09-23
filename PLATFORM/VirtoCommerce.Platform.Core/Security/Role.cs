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
                    //Add not scope bounded permission as it
                    var retVal = Permissions.Where(x=>!x.ScopeBounded).Select(x => x.Id).ToList();                    
                    //Add scope bounded permission with scope joined
                    retVal.AddRange(Permissions.Where(x=> x.ScopeBounded).Select(x => x.Id).LeftJoin(Scopes, ":"));
                    return retVal;
                }
            }
            return Enumerable.Empty<string>();
        }

    }
}
