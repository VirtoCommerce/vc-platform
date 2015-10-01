using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RolePermissionEntity : AuditableEntity
    {
        public RolePermissionEntity()
        {
              Scopes = new NullCollection<PermissionScopeEntity>();
        }

        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
        public virtual ObservableCollection<PermissionScopeEntity> Scopes { get; set; }
    }
}
