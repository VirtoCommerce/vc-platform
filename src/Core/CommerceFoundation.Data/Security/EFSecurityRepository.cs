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
		private readonly ISecurityEntityFactory _entityFactory;

		public EFSecurityRepository()
		{
		}

		public EFSecurityRepository(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFSecurityRepository>(null);
		}

		[InjectionConstructor]
        public EFSecurityRepository(ISecurityEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : this(SecurityConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
		{
		}

        public EFSecurityRepository(string nameOrConnectionString, ISecurityEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString, entityFactory, interceptors: interceptors)
        {
            _entityFactory = entityFactory;

            Database.SetInitializer(new ValidateDatabaseInitializer<EFSecurityRepository>());

            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Permission>(modelBuilder, toTable: "Permission");
			MapEntity<Role>(modelBuilder, toTable: "Role");
			MapEntity<RoleAssignment>(modelBuilder, toTable: "RoleAssignment");
			MapEntity<RolePermission>(modelBuilder, toTable: "RolePermission");
			MapEntity<Account>(modelBuilder, toTable: "Account");

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
		#endregion


	   
	}
}
