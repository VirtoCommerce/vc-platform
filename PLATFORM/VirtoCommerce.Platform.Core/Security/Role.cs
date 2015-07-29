using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Platform.Core.Security
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Permission[] Permissions { get; set; }
    }
}
