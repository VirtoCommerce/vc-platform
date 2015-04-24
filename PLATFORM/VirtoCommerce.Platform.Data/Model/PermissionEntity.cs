using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class PermissionEntity : Entity
    {
        public PermissionEntity()
        {
            RolePermissions = new ObservableCollection<RolePermissionEntity>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public ObservableCollection<RolePermissionEntity> RolePermissions { get; set; }
    }
}
