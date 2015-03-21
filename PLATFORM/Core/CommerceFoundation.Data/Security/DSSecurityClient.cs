using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data.Security
{
	public class DSSecurityClient : DSClientBase, ISecurityRepository
	{
		[InjectionConstructor]
		public DSSecurityClient(ISecurityEntityFactory factory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(SecurityConfiguration.Instance.Connection.DataServiceUri), factory, tokenInjector)
		{
		}

		public DSSecurityClient(Uri serviceUri, ISecurityEntityFactory factory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, factory, tokenInjector)
		{
		}

		#region ISecurityRepository Members

		public IQueryable<Permission> Permissions
		{
			get { return GetAsQueryable<Permission>(); }
		}

		public IQueryable<Role> Roles
		{
			get { return GetAsQueryable<Role>(); }
		}

		public IQueryable<RolePermission> RolePermissions
		{
			get { return GetAsQueryable<RolePermission>(); }
		}

		public IQueryable<RoleAssignment> RoleAssignments
		{
			get { return GetAsQueryable<RoleAssignment>(); }
		}

		public IQueryable<Account> Accounts
		{
			get { return GetAsQueryable<Account>(); }
		}

		public IQueryable<ApiAccount> ApiAccounts
		{
			get { return GetAsQueryable<ApiAccount>(); }
		}

		#endregion
	}
}
