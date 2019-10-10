using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Role : IdentityRole
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public string Description { get; set; }
        public IList<Permission> Permissions { get; set; }

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
    }
}
