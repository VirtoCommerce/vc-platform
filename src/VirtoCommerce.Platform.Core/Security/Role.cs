using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Role : IdentityRole, ICloneable
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public string Description { get; set; }
        public IList<Permission> Permissions { get; set; }
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        public virtual void Patch(Role target)
        {
            target.Name = Name;
            target.NormalizedName = NormalizedName;
            target.ConcurrencyStamp = ConcurrencyStamp;
            target.Description = Description;

            if (!Permissions.IsNullCollection())
            {
                Permissions.Patch(target.Permissions, (sourcePermission, targetPermission) => sourcePermission.Patch(targetPermission));
            }
        }

        #region ICloneable members
        public virtual object Clone()
        {
            var result = MemberwiseClone() as Role;

            result.Permissions = Permissions?.Select(x => x.Clone()).OfType<Permission>().ToList();

            return result;
        }
        #endregion
    }
}
