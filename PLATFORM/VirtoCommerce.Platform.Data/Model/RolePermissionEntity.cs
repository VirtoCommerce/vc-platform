using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RolePermissionEntity : Entity
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }

        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
