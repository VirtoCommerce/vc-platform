using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.ManagementClient.Security.Model
{
    public class PermissionGroup
    {
        public string DisplayName { get; set; }
        public Permission[] AllAvailablePermissions { get; private set; }

        public PermissionGroup(string displayName, Permission[] availablePermissions)
        {
            DisplayName = displayName;
            AllAvailablePermissions = availablePermissions;
        }
    }
}
