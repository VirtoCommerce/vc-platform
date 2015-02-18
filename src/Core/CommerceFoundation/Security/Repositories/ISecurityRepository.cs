using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.Foundation.Security.Repositories
{
	public interface ISecurityRepository : IRepository
	{
		IQueryable<Permission> Permissions { get; }
		IQueryable<Role> Roles { get; }
		IQueryable<RoleAssignment> RoleAssignments { get; }
		IQueryable<RolePermission> RolePermissions { get; }
		IQueryable<Account> Accounts { get; }
		IQueryable<ApiAccount> ApiAccounts { get; }
	}
}
