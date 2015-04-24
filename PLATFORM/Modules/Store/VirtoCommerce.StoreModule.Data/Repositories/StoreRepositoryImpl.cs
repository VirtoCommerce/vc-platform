using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.StoreModule.Data.Model;

namespace VirtoCommerce.StoreModule.Data.Repositories
{
	public class StoreRepositoryImpl : EFRepositoryBase, IStoreRepository
	{
		public StoreRepositoryImpl()
		{
		}

		public StoreRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public StoreRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		
			modelBuilder.Entity<FulfillmentCenter>().HasKey(x => x.Id).Property(x => x.Id)
									.HasColumnName("FulfillmentCenterId");
			modelBuilder.Entity<Store>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("StoreId");
			modelBuilder.Entity<StoreCurrency>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("StoreCurrencyId");
			modelBuilder.Entity<StoreLanguage>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("StoreLanguageId");
			modelBuilder.Entity<StorePaymentGateway>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("StorePaymentGatewayId");
			modelBuilder.Entity<StoreSetting>().HasKey(x => x.Id).Property(x => x.Id)
									.HasColumnName("StoreSettingId");

			MapEntity<FulfillmentCenter>(modelBuilder, toTable: "vc_FulfillmentCenter");
			MapEntity<Store>(modelBuilder, toTable: "vc_Store");
			MapEntity<StoreCurrency>(modelBuilder, toTable: "vc_StoreCurrency");
			MapEntity<StoreLanguage>(modelBuilder, toTable: "vc_StoreLanguage");
			MapEntity<StorePaymentGateway>(modelBuilder, toTable: "vc_StorePaymentGateway");
			MapEntity<StoreSetting>(modelBuilder, toTable: "vc_StoreSetting");

			base.OnModelCreating(modelBuilder);
		}

		#region IStoreRepository Members

		public Store GetStoreById(string id)
		{
			var retVal = Stores.Where(x => x.Id == id).Include(x => x.Settings)
														 .Include(x => x.ReturnsFulfillmentCenter)
														 .Include(x => x.Languages)
														 .Include(x => x.Currencies)
														 .Include(x=> x.PaymentGateways)
														 .Include(x => x.FulfillmentCenter);
			return retVal.FirstOrDefault();
		}

		public IQueryable<Store> Stores
		{
			get { return GetAsQueryable<Store>(); }
		}

		#endregion

		
	}

}
