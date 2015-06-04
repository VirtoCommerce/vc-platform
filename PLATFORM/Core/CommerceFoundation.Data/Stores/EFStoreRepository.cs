using System.Linq;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores;
using VirtoCommerce.Foundation.Stores.Model;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Stores
{
	public class EFStoreRepository : EFRepositoryBase, IStoreRepository, IFulfillmentCenterRepository
	{
		private readonly IStoreEntityFactory _entityFactory;

		public EFStoreRepository()
		{
		}

		public EFStoreRepository(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFStoreRepository>(null);
		}

		[InjectionConstructor]
		public EFStoreRepository(IStoreEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(StoreConfiguration.Instance.Connection.SqlConnectionStringName, interceptors: interceptors)
		{
			_entityFactory = entityFactory;

			Database.SetInitializer(new ValidateDatabaseInitializer<EFStoreRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		public EFStoreRepository(string nameOrConnectionString, IStoreEntityFactory entityFactory,
			IInterceptor[] interceptors = null)
			: base(nameOrConnectionString, interceptors: interceptors)
		{
			_entityFactory = entityFactory;
		    Database.SetInitializer(new ValidateDatabaseInitializer<EFStoreRepository>());

			Configuration.AutoDetectChangesEnabled = true;
			Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<FulfillmentCenter>(modelBuilder, toTable: "FulfillmentCenter");
			MapEntity<Store>(modelBuilder, toTable: "Store");
			MapEntity<StoreCardType>(modelBuilder, toTable: "StoreCardType");
			MapEntity<StoreCurrency>(modelBuilder, toTable: "StoreCurrency");
			MapEntity<StoreLanguage>(modelBuilder, toTable: "StoreLanguage");
			MapEntity<StoreLinkedStore>(modelBuilder, toTable: "StoreLinkedStore");
			MapEntity<StorePaymentGateway>(modelBuilder, toTable: "StorePaymentGateway");
			MapEntity<StoreTaxCode>(modelBuilder, toTable: "StoreTaxCode");
			MapEntity<StoreTaxJurisdiction>(modelBuilder, toTable: "StoreTaxJurisdiction");
			MapEntity<StoreSetting>(modelBuilder, toTable: "StoreSetting");

			//modelBuilder.Entity<Store>().HasRequired(p => p.FulfillmentCenter).WithMany().WillCascadeOnDelete(false);
			//modelBuilder.Entity<Store>().HasRequired(p => p.ReturnsFulfillmentCenter).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<StoreLinkedStore>().HasRequired(p => p.LinkedStore).WithMany().WillCascadeOnDelete(false);


			base.OnModelCreating(modelBuilder);
		}

		#region IStoreRepository Members

		public IQueryable<Store> Stores
		{
			get { return GetAsQueryable<Store>(); }
		}

		public IQueryable<StoreTaxCode> StoreTaxCodes
		{
			get { return GetAsQueryable<StoreTaxCode>(); }
		}

		public IQueryable<StoreTaxJurisdiction> StoreTaxJurisdictions
		{
			get { return GetAsQueryable<StoreTaxJurisdiction>(); }
		}

		public IQueryable<StoreCardType> StoreCardTypes
		{
			get { return GetAsQueryable<StoreCardType>(); }
		}

		public IQueryable<StorePaymentGateway> StorePaymentGateways
		{
			get { return GetAsQueryable<StorePaymentGateway>(); }
		}

		public IQueryable<StoreSetting> StoreSettings
		{
			get { return GetAsQueryable<StoreSetting>(); }
		}

		public IQueryable<StoreLinkedStore> StoreLinkedStores
		{
			get { return GetAsQueryable<StoreLinkedStore>(); }
		}

		public IQueryable<StoreCurrency> StoreCurrencies
		{
			get { return GetAsQueryable<StoreCurrency>(); }
		}

		public IQueryable<StoreLanguage> StoreLanguages
		{
			get { return GetAsQueryable<StoreLanguage>(); }
		}

		#endregion

		#region IFulfillmentCenterRepository Members

		public IQueryable<FulfillmentCenter> FulfillmentCenters
		{
			get { return GetAsQueryable<FulfillmentCenter>(); }
		}

		#endregion
	}
}
