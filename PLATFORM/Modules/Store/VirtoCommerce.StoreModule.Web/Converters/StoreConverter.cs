using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class StoreConverter
	{
		public static webModel.Store ToWebModel(this coreModel.Store store)
		{
			var retVal = new webModel.Store();
			retVal.InjectFrom(store);

			retVal.DefaultCurrency = store.DefaultCurrency;
			retVal.StoreState = store.StoreState;
			if (store.Settings != null)
				retVal.Settings = store.Settings.Select(x => x.ToWebModel()).ToList();
			if (store.ShipmentGateways != null)
				retVal.ShipmentGateways = store.ShipmentGateways;
			if (store.PaymentGateways != null)
				retVal.PaymentGateways = store.PaymentGateways;
			if (store.Languages != null)
				retVal.Languages = store.Languages;
			if (store.Currencies != null)
				retVal.Currencies = store.Currencies;
			if (store.ReturnsFulfillmentCenter != null)
				retVal.ReturnsFulfillmentCenter = store.ReturnsFulfillmentCenter.ToWebModel();
			if (store.FulfillmentCenter != null)
				retVal.FulfillmentCenter = store.FulfillmentCenter.ToWebModel();
			if (store.SeoInfos != null)
				retVal.SeoInfos = store.SeoInfos.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.Store ToCoreModel(this webModel.Store store)
		{
			var retVal = new coreModel.Store();
			retVal.InjectFrom(store);

			retVal.StoreState = store.StoreState;
			if (store.Settings != null)
				retVal.Settings = store.Settings.Select(x => x.ToCoreModel()).ToList();
			if (store.ShipmentGateways != null)
				retVal.ShipmentGateways = store.ShipmentGateways;
			if (store.PaymentGateways != null)
				retVal.PaymentGateways = store.PaymentGateways;
			if (store.Languages != null)
				retVal.Languages = store.Languages;
			if (store.Currencies != null)
				retVal.Currencies = store.Currencies;
			if (store.ReturnsFulfillmentCenter != null)
				retVal.ReturnsFulfillmentCenter = store.ReturnsFulfillmentCenter.ToCoreModel();
			if (store.FulfillmentCenter != null)
				retVal.FulfillmentCenter = store.FulfillmentCenter.ToCoreModel();
			if (store.SeoInfos != null)
				retVal.SeoInfos = store.SeoInfos.Select(x => x.ToCoreModel()).ToList();

			return retVal;
		}


	}
}
