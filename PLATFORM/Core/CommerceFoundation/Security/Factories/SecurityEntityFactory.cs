using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.Foundation.Security.Factories
{
	public class SecurityEntityFactory : FactoryBase, ISecurityEntityFactory
    {
        public SecurityEntityFactory()
		{
			RegisterStorageType(typeof(Role), "Role");
			RegisterStorageType(typeof(RoleAssignment), "RoleAssignment");
            RegisterStorageType(typeof(RolePermission), "RolePermission");
			RegisterStorageType(typeof(Permission), "Permission");
            RegisterStorageType(typeof(Account), "Account");
		}
	}
}
