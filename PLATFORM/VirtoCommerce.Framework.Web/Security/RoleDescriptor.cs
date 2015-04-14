namespace VirtoCommerce.Framework.Web.Security
{
    public class RoleDescriptor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PermissionDescriptor[] Permissions { get; set; }
    }
}
