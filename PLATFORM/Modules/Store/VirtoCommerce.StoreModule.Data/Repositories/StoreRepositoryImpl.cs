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

		public StoreRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Configuration.LazyLoadingEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            #region Store
            modelBuilder.Entity<Store>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Store>().ToTable("Store");
            #endregion

            #region StoreCurrency
            modelBuilder.Entity<StoreCurrency>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StoreCurrency>().ToTable("StoreCurrency");

            modelBuilder.Entity<StoreCurrency>().HasRequired(x => x.Store)
                                   .WithMany(x => x.Currencies)
                                   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion

            #region StoreLanguage
            modelBuilder.Entity<StoreLanguage>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StoreLanguage>().ToTable("StoreLanguage");

            modelBuilder.Entity<StoreLanguage>().HasRequired(x => x.Store)
                                   .WithMany(x => x.Languages)
                                   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion

            #region StoreTrustedGroups
            modelBuilder.Entity<StoreTrustedGroup>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StoreTrustedGroup>().ToTable("StoreTrustedGroup");

            modelBuilder.Entity<StoreTrustedGroup>().HasRequired(x => x.Store)
                                   .WithMany(x => x.TrustedGroups)
								   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion

            #region StorePaymentMethod
            modelBuilder.Entity<StorePaymentMethod>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StorePaymentMethod>().ToTable("StorePaymentMethod");

			modelBuilder.Entity<StorePaymentMethod>().HasRequired(x => x.Store)
							   .WithMany(x => x.PaymentMethods)
							   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion

            #region StoreShippingMethod
            modelBuilder.Entity<StoreShippingMethod>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StoreShippingMethod>().ToTable("StoreShippingMethod");

            modelBuilder.Entity<StoreShippingMethod>().HasRequired(x => x.Store)
                                   .WithMany(x => x.ShippingMethods)
                                   .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion

            #region StoreTaxProvider
            modelBuilder.Entity<StoreTaxProvider>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<StoreTaxProvider>().ToTable("StoreTaxProvider");

            modelBuilder.Entity<StoreTaxProvider>().HasRequired(x => x.Store)
                                 .WithMany(x => x.TaxProviders)
                                 .HasForeignKey(x => x.StoreId).WillCascadeOnDelete(true);
            #endregion


            base.OnModelCreating(modelBuilder);
		}

		#region IStoreRepository Members

		public Store[] GetStoresByIds(string[] ids)
		{
            var retVal = Stores.Where(x => ids.Contains(x.Id))
                               .Include(x => x.Languages)
                               .Include(x => x.Currencies).Include(x => x.TrustedGroups)
                               .ToArray();
            var paymentMethods = StorePaymentMethods.Where(x => ids.Contains(x.StoreId)).ToArray();
            var shipmentMethods = StoreShippingMethods.Where(x => ids.Contains(x.StoreId)).ToArray();
            var taxProviders = StoreTaxProviders.Where(x => ids.Contains(x.StoreId)).ToArray();
                           
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
