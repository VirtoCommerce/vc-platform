using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Stores
{
	public class DSStoreClient : DSClientBase, IStoreRepository, IFulfillmentCenterRepository
	{
		[InjectionConstructor]
		public DSStoreClient(IStoreEntityFactory factory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(StoreConfiguration.Instance.Connection.DataServiceUri), factory, tokenInjector)
		{
		}

		public DSStoreClient(Uri serviceUri, IStoreEntityFactory factory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, factory, tokenInjector)
		{
		}


		#region IStoreRepository Members

		public IQueryable<Store> Stores
		{
			get { return GetAsQueryable<Store>(); }
		}

		public IQueryable<StoreCurrency> StoreCurrencies
		{
			get { return GetAsQueryable<StoreCurrency>(); }
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
