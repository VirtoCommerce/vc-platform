using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories;
using VirtoCommerce.Foundation.Inventories.Model;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Inventories
{
	public class EFInventoryRepository: EFRepositoryBase, IInventoryRepository
	{
		private readonly IInventoryEntityFactory _entityFactory;

        public EFInventoryRepository()
        {
        }

		public EFInventoryRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFInventoryRepository>(null);
        }

		public EFInventoryRepository(string connectionStringName, IInventoryEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(connectionStringName, entityFactory, interceptors: interceptors)
		{
			_entityFactory = entityFactory;
			Database.SetInitializer(new ValidateDatabaseInitializer<EFInventoryRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

        [InjectionConstructor]
        public EFInventoryRepository(IInventoryEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(InventoryConfiguration.Instance.Connection.SqlConnectionStringName, factory: entityFactory, interceptors: interceptors)
		{
			_entityFactory = entityFactory;
            Database.SetInitializer(new ValidateDatabaseInitializer<EFInventoryRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Inventory>(modelBuilder, toTable: "Inventory");

			base.OnModelCreating(modelBuilder);
		}



		#region IInventoryRepository Members

		public IQueryable<Inventory> Inventories
		{
			get { return GetAsQueryable<Inventory>(); }
		}

		#endregion
	}
}
