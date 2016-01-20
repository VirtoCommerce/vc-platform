﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Tax.Model;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class StoreConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Store ToCoreModel(this dataModel.Store dbStore, ShippingMethod[] shippingMethods, PaymentMethod[] paymentMethods, TaxProvider[] taxProviders)
		{
			if (dbStore == null)
				throw new ArgumentNullException("dbStore");

			var retVal = new coreModel.Store();
			retVal.InjectFrom(dbStore);
	
			retVal.DefaultCurrency = dbStore.DefaultCurrency;
			retVal.StoreState = (coreModel.StoreState)dbStore.StoreState;
			
			retVal.Languages = dbStore.Languages.Select(x => x.LanguageCode).ToList();
            retVal.Currencies = dbStore.Currencies.Select(x => x.CurrencyCode).ToList();

            //Payment methods need return only contains in registered
            retVal.PaymentMethods = paymentMethods;
			foreach (var paymentMethod in paymentMethods)
			{
				var dbStoredPaymentMethod = dbStore.PaymentMethods.FirstOrDefault(x => x.Code == paymentMethod.Code);
				if (dbStoredPaymentMethod != null)
				{
					paymentMethod.InjectFrom(dbStoredPaymentMethod);
				}
			}

			//Shipping methods need return only contains in registered
			retVal.ShippingMethods = shippingMethods;
			foreach (var shippingMethod in shippingMethods)
			{
				var dbStoredShippingMethod = dbStore.ShippingMethods.FirstOrDefault(x => x.Code == shippingMethod.Code);
				if(dbStoredShippingMethod != null)
				{
					shippingMethod.InjectFrom(dbStoredShippingMethod);
				}
			}

            //Tax providers need return only contains in registered
            retVal.TaxProviders = taxProviders;
            foreach (var taxProvider in taxProviders)
            {
                var dbStoredTaxProvider = dbStore.TaxProviders.FirstOrDefault(x => x.Code == taxProvider.Code);
                if (dbStoredTaxProvider != null)
                {
                    taxProvider.InjectFrom(dbStoredTaxProvider);
                }
            }
            return retVal;

		}


		public static dataModel.Store ToDataModel(this coreModel.Store store)
		{
			if (store == null)
				throw new ArgumentNullException("store");

			var retVal = new dataModel.Store();

			retVal.InjectFrom(store);
			retVal.StoreState = (int)store.StoreState;
			
			if(store.DefaultCurrency != null)
			{
				retVal.DefaultCurrency = store.DefaultCurrency.ToString();
			}
			if (store.FulfillmentCenter != null)
			{
				retVal.FulfillmentCenterId = store.FulfillmentCenter.Id;
			}
			if (store.ReturnsFulfillmentCenter != null)
			{
				retVal.ReturnsFulfillmentCenterId = store.ReturnsFulfillmentCenter.Id;
			}
			if (store.Languages != null)
			{
				retVal.Languages = new ObservableCollection<dataModel.StoreLanguage>(store.Languages.Select(x=> new dataModel.StoreLanguage
					{
						LanguageCode = x,
						StoreId = retVal.Id
					}));
			}
		
			if(store.Currencies != null)
			{
				retVal.Currencies = new ObservableCollection<dataModel.StoreCurrency>(store.Currencies.Select(x => new dataModel.StoreCurrency
				{
					CurrencyCode = x.ToString(),
					StoreId = retVal.Id
				}));
			}
			
			if (store.ShippingMethods != null)
			{
				retVal.ShippingMethods = new ObservableCollection<dataModel.StoreShippingMethod>(store.ShippingMethods.Select(x => x.ToDataModel()));
			}
			if (store.PaymentMethods != null)
			{
				retVal.PaymentMethods = new ObservableCollection<dataModel.StorePaymentMethod>(store.PaymentMethods.Select(x => x.ToDataModel()));
			}
            if (store.TaxProviders != null)
            {
                retVal.TaxProviders = new ObservableCollection<dataModel.StoreTaxProvider>(store.TaxProviders.Select(x => x.ToDataModel()));
            }
            return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.Store source, dataModel.Store target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjectionPolicy = new PatchInjection<dataModel.Store>(x=>x.FulfillmentCenterId, x=>x.ReturnsFulfillmentCenterId, 
																		   x => x.AdminEmail, x => x.Catalog,
																		   x => x.Country, x => x.CreditCardSavePolicy,
																		   x => x.DefaultCurrency, x => x.DefaultLanguage,
																		   x => x.Description, x => x.DisplayOutOfStock,
																		   x => x.Email, x => x.Name, x => x.Region, x => x.SecureUrl,
																		   x => x.TimeZone, x => x.Url, x=>x.StoreState);
			target.InjectFrom(patchInjectionPolicy, source);


			if (!source.Languages.IsNullCollection())
			{
				var languageComparer = AnonymousComparer.Create((dataModel.StoreLanguage x) => x.LanguageCode);
				source.Languages.Patch(target.Languages, languageComparer, 
									  (sourceLang, targetLang) =>  targetLang.LanguageCode = sourceLang.LanguageCode);
			}
			if (!source.Currencies.IsNullCollection())
			{
				var currencyComparer = AnonymousComparer.Create((dataModel.StoreCurrency x) => x.CurrencyCode);
				source.Currencies.Patch(target.Currencies, currencyComparer,
									  (sourceCurrency, targetCurrency) => targetCurrency.CurrencyCode = sourceCurrency.CurrencyCode);
			}
			if (!source.PaymentMethods.IsNullCollection())
			{
				var paymentComparer = AnonymousComparer.Create((dataModel.StorePaymentMethod x) => x.Code);
				source.PaymentMethods.Patch(target.PaymentMethods, paymentComparer,
									  (sourceMethod, targetMethod) => sourceMethod.Patch(targetMethod));
			}
			if (!source.ShippingMethods.IsNullCollection())
			{
				var shippingComparer = AnonymousComparer.Create((dataModel.StoreShippingMethod x) => x.Code);
				source.ShippingMethods.Patch(target.ShippingMethods, shippingComparer,
									  (sourceMethod, targetMethod) => sourceMethod.Patch(targetMethod));
			}
            if (!source.TaxProviders.IsNullCollection())
            {
                var shippingComparer = AnonymousComparer.Create((dataModel.StoreTaxProvider x) => x.Code);
                source.TaxProviders.Patch(target.TaxProviders, shippingComparer,
                                      (sourceProvider, targetProvider) => sourceProvider.Patch(targetProvider));
            }
        }
		
		
	}
}
