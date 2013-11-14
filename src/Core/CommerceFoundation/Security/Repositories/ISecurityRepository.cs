using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Security.Repositories
{
	public interface ISecurityRepository : IRepository
	{
		IQueryable<Permission> Permissions { get; }
		IQueryable<Role> Roles { get; }
		IQueryable<RoleAssignment> RoleAssignments { get; }
        IQueryable<RolePermission> RolePermissions { get; } 
        IQueryable<Account> Accounts { get; }
	}
}
