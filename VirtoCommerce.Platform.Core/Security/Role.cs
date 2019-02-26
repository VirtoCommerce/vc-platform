using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Role : Entity, ICloneable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Permission[] Permissions { get; set; }

        public virtual object Clone()
        {
            var clone = (Role) MemberwiseClone();

            if (Permissions != null)
            {
                clone.Permissions = Permissions.Select(x => x.Clone() as Permission).ToArray();
            }

            return clone;
        }
    }
}
