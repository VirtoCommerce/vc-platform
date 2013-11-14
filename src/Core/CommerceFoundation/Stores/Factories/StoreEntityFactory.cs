using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Model;


namespace VirtoCommerce.Foundation.Stores.Factories
{
	public class StoreEntityFactory : FactoryBase, IStoreEntityFactory
	{
		public StoreEntityFactory()
		{
			RegisterStorageType(typeof(Store), "Store");
			RegisterStorageType(typeof(StoreCardType), "StoreCardType");
			RegisterStorageType(typeof(StoreCurrency), "StoreCurrency");
			RegisterStorageType(typeof(StoreLanguage), "StoreLanguage");
			RegisterStorageType(typeof(StoreSetting), "StoreSetting");
			RegisterStorageType(typeof(StoreLinkedStore), "StoreLinkedStore");
			RegisterStorageType(typeof(StorePaymentGateway), "StorePaymentGateway");
			RegisterStorageType(typeof(StoreTaxCode), "StoreTaxCode");
			RegisterStorageType(typeof(StoreTaxJurisdiction), "StoreTaxJurisdiction");
			RegisterStorageType(typeof(FulfillmentCenter), "FulfillmentCenter");
		}
	}
}
