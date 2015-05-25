using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PaypalStoreSettingInitializer
	{
		private IStoreService _service;

		public PaypalStoreSettingInitializer(IStoreService service)
		{
			if(service == null)
				throw new ArgumentNullException("service");

			_service = service;
		}

		public void Initialize()
		{
			var stores = _service.GetStoreList();

			foreach(var store in stores)
			{
				CheckAndAddSetting("Paypal.Mode", SettingValueType.ShortText, store);
				CheckAndAddSetting("Paypal.APIUsername", SettingValueType.ShortText, store);
				CheckAndAddSetting("Paypal.APIPassword", SettingValueType.ShortText, store);
				CheckAndAddSetting("Paypal.APISignature", SettingValueType.ShortText, store);

				_service.Update(new Store[] { store });
			}
		}

		private void CheckAndAddSetting(string settingName, SettingValueType type, Store store)
		{
			//var setting = store.Settings.FirstOrDefault(s => s.Name == settingName);

			//if(setting == null)
			//{
			//	//store.Settings.Add(new StoreSetting
			//	//	{
			//	//		Name = settingName,
			//	//		Value = string.Empty,
			//	//		ValueType = type
			//	//	});
			//}
		}
	}
}
