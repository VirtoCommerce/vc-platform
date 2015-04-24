using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleEntity : Entity
    {
        public RoleEntity()
        {
            RolePermissions = new ObservableCollection<RolePermissionEntity>();
            RoleAssignments = new ObservableCollection<RoleAssignmentEntity>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public ObservableCollection<RolePermissionEntity> RolePermissions { get; set; }
        public ObservableCollection<RoleAssignmentEntity> RoleAssignments { get; set; }
    }
}
