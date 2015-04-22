namespace VirtoCommerce.Platform.Core.Security
{
    public class RoleDescriptor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PermissionDescriptor[] Permissions { get; set; }
    }
}
