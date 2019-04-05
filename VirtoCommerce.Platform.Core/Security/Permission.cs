﻿using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Permission : Entity, ICloneable
    {
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

        public ICollection<PermissionScope> AssignedScopes { get; set; }

        public ICollection<PermissionScope> AvailableScopes { get; set; }
        /// <summary>
        /// Generate permissions string with scope combination
        /// </summary>
        public IEnumerable<string> GetPermissionWithScopeCombinationNames()
        {
            var retVal = new List<string>();
            if (AssignedScopes != null && AssignedScopes.Any())
            {
                retVal.AddRange(AssignedScopes.Select(x => Id + ":" + x.ToString()));
            }
            else
            {
                retVal.Add(Id);
            }
            return retVal;
        }

        public virtual object Clone()
        {
            var clone = (Permission)MemberwiseClone();

            if (AssignedScopes != null)
            {
                clone.AssignedScopes = new List<PermissionScope>(AssignedScopes.Select(x => x.Clone() as PermissionScope));
            }

            if (AvailableScopes != null)
            {
                clone.AvailableScopes = new List<PermissionScope>(AvailableScopes.Select(x => x.Clone() as PermissionScope));
            }

            return clone;
        }
    }
}
