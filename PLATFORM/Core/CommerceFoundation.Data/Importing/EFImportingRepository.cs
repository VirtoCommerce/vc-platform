using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Foundation.Importing;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.Foundation.Data.Importing
{
	public class EFImportingRepository : EFRepositoryBase, IImportRepository
	{
		private readonly IImportJobEntityFactory _entityFactory;

        public EFImportingRepository()
        {
        }

        public EFImportingRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFImportingRepository>(null);
        }

        [InjectionConstructor]
        public EFImportingRepository(IImportJobEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(ImportConfiguration.Instance.Connection.SqlConnectionStringName, factory: entityFactory, interceptors: interceptors)
		{
			_entityFactory = entityFactory;
            Database.SetInitializer(new ValidateDatabaseInitializer<EFImportingRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			MapEntity<MappingItem>(modelBuilder, toTable: "MappingItem");
			MapEntity<ImportJob>(modelBuilder, toTable: "ImportJob");

			base.OnModelCreating(modelBuilder);

		}


		#region IImportingRepository Members

		public IQueryable<MappingItem> MappingItems
		{
			get { return GetAsQueryable<MappingItem>(); }
		}

		public IQueryable<ImportJob> ImportJobs
		{
			get { return GetAsQueryable<ImportJob>(); }
		}

		#endregion
	}
}
