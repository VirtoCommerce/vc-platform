using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class PermissionEntity : AuditableEntity
    {
        public PermissionEntity()
        {
            RolePermissions = new ObservableCollection<RolePermissionEntity>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

		public virtual ObservableCollection<RolePermissionEntity> RolePermissions { get; set; }
    }
}
