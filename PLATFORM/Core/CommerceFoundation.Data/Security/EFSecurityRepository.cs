using System.Linq;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Model;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Security
{
	public class EFSecurityRepository : EFRepositoryBase, ISecurityRepository
	{
		public EFSecurityRepository()
		{
		}

		public EFSecurityRepository(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			Configuration.AutoDetectChangesEnabled = true;
			Configuration.ProxyCreationEnabled = false;
		}

		[InjectionConstructor]
		public EFSecurityRepository(ISecurityEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: this(SecurityConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
		{
		}

		public EFSecurityRepository(string nameOrConnectionString, ISecurityEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(nameOrConnectionString, entityFactory, interceptors: interceptors)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<EFSecurityRepository>());

			Configuration.AutoDetectChangesEnabled = true;
			Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Permission>(modelBuilder, toTable: "Permission");
			MapEntity<Role>(modelBuilder, toTable: "Role");
			MapEntity<RoleAssignment>(modelBuilder, toTable: "RoleAssignment");
			MapEntity<RolePermission>(modelBuilder, toTable: "RolePermission");
			MapEntity<Account>(modelBuilder, toTable: "Account");
			MapEntity<ApiAccount>(modelBuilder, toTable: "ApiAccount");

			modelBuilder.Entity<RolePermission>()
				.HasRequired(o => o.Permission)
				.WithMany(o => o.RolePermissions);

			modelBuilder.Entity<RolePermission>()
				.HasRequired(o => o.Role)
				.WithMany(o => o.RolePermissions);

			base.OnModelCreating(modelBuilder);
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
