using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Stores.Repositories
{
	public interface IStoreRepository : IRepository
	{
		IQueryable<Store> Stores { get; }
        IQueryable<StoreCurrency> StoreCurrencies { get; }
        IQueryable<StoreTaxCode> StoreTaxCodes { get; }
		IQueryable<StoreTaxJurisdiction> StoreTaxJurisdictions { get; }
		IQueryable<StoreCardType> StoreCardTypes { get; }
		IQueryable<StorePaymentGateway> StorePaymentGateways { get; }
		IQueryable<StoreSetting> StoreSettings { get; }
		IQueryable<StoreLinkedStore> StoreLinkedStores { get; }
		IQueryable<StoreLanguage> StoreLanguages { get; } 
	}
}
