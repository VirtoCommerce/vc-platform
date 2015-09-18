using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class PermissionEntity : AuditableEntity
    {
        public PermissionEntity()
        {
            RolePermissions = new ObservableCollection<RolePermissionEntity>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public string Description { get; set; }

		public virtual ObservableCollection<RolePermissionEntity> RolePermissions { get; set; }
    }
}
