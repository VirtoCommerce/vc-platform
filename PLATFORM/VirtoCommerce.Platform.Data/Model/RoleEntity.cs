using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleEntity : AuditableEntity
    {
        public RoleEntity()
        {
            RolePermissions = new NullCollection<RolePermissionEntity>();
			RoleAssignments = new NullCollection<RoleAssignmentEntity>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ObservableCollection<RolePermissionEntity> RolePermissions { get; set; }
		public virtual ObservableCollection<RoleAssignmentEntity> RoleAssignments { get; set; }
    }
}
