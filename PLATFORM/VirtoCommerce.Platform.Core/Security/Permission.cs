using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Permission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Id of the module which has registered this permission.
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// Display name of the group to which this permission belongs. The '|' character is used to separate Child and parent groups.
        /// </summary>
        public string GroupName { get; set; }

        public ICollection<string> Scopes { get; set; }

        /// <summary>
        /// Generate permissions string with scope combination
        /// </summary>
        public IEnumerable<string> GetPermissionWithScopeCombinationNames()
        {
            var retVal = new List<string>();
            retVal.Add(Name);
            if(Scopes != null)
            {
                retVal.AddRange(Scopes.Select(x => Name + ":" + x));
            }
            return retVal;
        }

    }
}
