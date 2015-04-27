namespace VirtoCommerce.Platform.Core.Security
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Permission[] Permissions { get; set; }
    }
}
