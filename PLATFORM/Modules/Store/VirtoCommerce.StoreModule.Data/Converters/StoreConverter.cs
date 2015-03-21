using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.Foundation.Stores.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class StoreConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Store ToCoreModel(this foundationModel.Store dbStore)
		{
			if (dbStore == null)
				throw new ArgumentNullException("dbStore");

			var retVal = new coreModel.Store();
			retVal.InjectFrom(dbStore);
			retVal.Id = dbStore.StoreId;
	
			retVal.CreatedDate = dbStore.Created.Value;
			retVal.ModifiedDate = dbStore.LastModified;

			if(dbStore.DefaultCurrency != null)
			{
				retVal.DefaultCurrency = (CurrencyCodes?)Enum.Parse(typeof(CurrencyCodes), dbStore.DefaultCurrency, true);
			}
			retVal.StoreState = (coreModel.StoreState)dbStore.StoreState;
			if(dbStore.FulfillmentCenter != null)
			{
				retVal.FulfillmentCenter = dbStore.FulfillmentCenter.ToCoreModel();
			}
			if (dbStore.ReturnsFulfillmentCenter != null)
			{
				retVal.ReturnsFulfillmentCenter = dbStore.ReturnsFulfillmentCenter.ToCoreModel();
			}
			retVal.Settings = dbStore.Settings.Select(x => x.ToCoreModel()).ToList();
			retVal.Languages = dbStore.Languages.Select(x => x.LanguageCode).ToList();
			retVal.Currencies = dbStore.Currencies.Select(x => (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), x.CurrencyCode, true)).ToList();
			retVal.PaymentGateways = dbStore.PaymentGateways.Select(x => x.PaymentGateway).ToList();

			return retVal;

		}


		public static foundationModel.Store ToFoundation(this coreModel.Store store)
		{
			if (store == null)
				throw new ArgumentNullException("store");

			var retVal = new foundationModel.Store();

			retVal.InjectFrom(store);

			if(store.Id != null)
			{
				retVal.StoreId = store.Id;
			}

			retVal.StoreState = (int)store.StoreState;
			
			if(store.DefaultCurrency != null)
			{
				retVal.DefaultCurrency = store.DefaultCurrency.ToString();
			}
			if (store.FulfillmentCenter != null)
			{
				retVal.FulfillmentCenter = store.FulfillmentCenter.ToFoundation();
				retVal.FulfillmentCenterId = retVal.FulfillmentCenter.FulfillmentCenterId;
			}
			if (store.ReturnsFulfillmentCenter != null)
			{
				retVal.ReturnsFulfillmentCenter = store.ReturnsFulfillmentCenter.ToFoundation();
				retVal.ReturnsFulfillmentCenterId = retVal.ReturnsFulfillmentCenter.FulfillmentCenterId;
			}
			retVal.Languages = new NullCollection<foundationModel.StoreLanguage>();
			if (store.Languages != null)
			{
				retVal.Languages = new ObservableCollection<foundationModel.StoreLanguage>(store.Languages.Select(x=> new foundationModel.StoreLanguage
					{
						LanguageCode = x,
						StoreId = retVal.StoreId
					}));
			}
			retVal.Settings = new NullCollection<foundationModel.StoreSetting>();
			if(store.Settings != null)
			{
				retVal.Settings = new ObservableCollection<foundationModel.StoreSetting>(store.Settings.Select(x=>x.ToFoundation()));
				foreach(var setting in retVal.Settings)
				{
					setting.StoreId = retVal.StoreId;
				}
			}
			retVal.Currencies = new NullCollection<foundationModel.StoreCurrency>();
			if(store.Currencies != null)
			{
				retVal.Currencies = new ObservableCollection<foundationModel.StoreCurrency>(store.Currencies.Select(x => new foundationModel.StoreCurrency
				{
					CurrencyCode = x.ToString(),
					StoreId = retVal.StoreId
				}));
			}
			retVal.PaymentGateways = new NullCollection<foundationModel.StorePaymentGateway>();
			if (store.PaymentGateways != null)
			{
				retVal.PaymentGateways = new ObservableCollection<foundationModel.StorePaymentGateway>(store.PaymentGateways.Select(x => new foundationModel.StorePaymentGateway
				{
					 PaymentGateway = x,
					 StoreId = retVal.StoreId
				}));
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Store source, foundationModel.Store target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjectionPolicy = new PatchInjection<foundationModel.Store>(x => x.AdminEmail, x => x.Catalog,
																		   x => x.Country, x => x.CreditCardSavePolicy,
																		   x => x.DefaultCurrency, x => x.DefaultLanguage,
																		   x => x.Description, x => x.DisplayOutOfStock,
																		   x => x.Email, x => x.Name, x => x.Region, x => x.SecureUrl,
																		   x => x.TimeZone, x => x.Url, x=>x.StoreState);
			target.InjectFrom(patchInjectionPolicy, source);


			if (source.FulfillmentCenter != null)
			{
				target.FulfillmentCenterId = source.FulfillmentCenter.FulfillmentCenterId;
			}

			if (source.ReturnsFulfillmentCenter != null)
			{
				target.ReturnsFulfillmentCenterId = source.ReturnsFulfillmentCenter.FulfillmentCenterId;
			}

			if (!source.Settings.IsNullCollection())
			{
				var settingComparer = AnonymousComparer.Create((foundationModel.StoreSetting x) => x.Name);
				source.Settings.Patch(target.Settings, settingComparer,	(sourceSetting, targetSetting) => sourceSetting.Patch(targetSetting));
			}
			if (!source.Languages.IsNullCollection())
			{
				var languageComparer = AnonymousComparer.Create((foundationModel.StoreLanguage x) => x.LanguageCode);
				source.Languages.Patch(target.Languages, languageComparer, 
									  (sourceLang, targetLang) =>  targetLang.LanguageCode = sourceLang.LanguageCode);
			}
			if (!source.Currencies.IsNullCollection())
			{
				var currencyComparer = AnonymousComparer.Create((foundationModel.StoreCurrency x) => x.CurrencyCode);
				source.Currencies.Patch(target.Currencies, currencyComparer,
									  (sourceCurrency, targetCurrency) => targetCurrency.CurrencyCode = sourceCurrency.CurrencyCode);
			}
			if (!source.PaymentGateways.IsNullCollection())
			{
				var paymentComparer = AnonymousComparer.Create((foundationModel.StorePaymentGateway x) => x.PaymentGateway);
				source.PaymentGateways.Patch(target.PaymentGateways, paymentComparer,
									  (sourceGateway, targetGateway) => targetGateway.PaymentGateway = sourceGateway.PaymentGateway);
			}
		}
		
		
	}
}
