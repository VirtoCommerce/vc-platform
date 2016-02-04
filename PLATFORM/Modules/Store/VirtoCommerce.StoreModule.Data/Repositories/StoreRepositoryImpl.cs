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
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Store>(modelBuilder, toTable: "Store");
			MapEntity<StoreCurrency>(modelBuilder, toTable: "StoreCurrency");
			MapEntity<StoreLanguage>(modelBuilder, toTable: "StoreLanguage");
			MapEntity<StorePaymentMethod>(modelBuilder, toTable: "StorePaymentMethod");
			MapEntity<StoreShippingMethod>(modelBuilder, toTable: "StoreShippingMethod");
            MapEntity<StoreTaxProvider>(modelBuilder, toTable: "StoreTaxProvider");

            modelBuilder.Entity<StoreShippingMethod>().HasRequired(x => x.Store)
								   .WithMany(x => x.ShippingMethods)
								   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);

			modelBuilder.Entity<StorePaymentMethod>().HasRequired(x => x.Store)
							   .WithMany(x => x.PaymentMethods)
							   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);

            modelBuilder.Entity<StoreTaxProvider>().HasRequired(x => x.Store)
                                 .WithMany(x => x.TaxProviders)
                                 .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);


            base.OnModelCreating(modelBuilder);
		}

		#region IStoreRepository Members

		public Store[] GetStoresByIds(string[] ids)
		{
            var retVal = Stores.Where(x => ids.Contains(x.Id)).Include(x => x.Languages)
                               .Include(x => x.Currencies).ToArray();
            var paymentMethods = StorePaymentMethods.Where(x => ids.Contains(x.Id)).ToArray();
            var shipmentMethods = StoreShippingMethods.Where(x => ids.Contains(x.Id)).ToArray();
            var taxProviders = StoreTaxProviders.Where(x => ids.Contains(x.Id)).ToArray();
                           
            return retVal;
		}

		public IQueryable<Store> Stores
		{
			get { return GetAsQueryable<Store>(); }
		}
        public IQueryable<StorePaymentMethod> StorePaymentMethods
        {
            get { return GetAsQueryable<StorePaymentMethod>(); }
        }
        public IQueryable<StoreShippingMethod> StoreShippingMethods
        {
            get { return GetAsQueryable<StoreShippingMethod>(); }
        }
        public IQueryable<StoreTaxProvider> StoreTaxProviders
        {
            get { return GetAsQueryable<StoreTaxProvider>(); }
        }
        #endregion


    }

}
