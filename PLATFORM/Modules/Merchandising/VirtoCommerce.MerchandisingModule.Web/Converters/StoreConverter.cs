using System.Linq;
using Omu.ValueInjecter;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class StoreConverter
	{
		public static webModel.Store ToWebModel(this coreModel.Store store)
		{
			var retVal = new webModel.Store();

			retVal.InjectFrom(store);

			if (store.Languages != null)
			{
				retVal.Languages = store.Languages.ToArray();
			}

			if (store.Currencies != null)
			{
				retVal.Currencies = store.Currencies.ToArray();
			}

		
			if (store.Settings != null)
			{
				retVal.Settings = new PropertyDictionary();

				foreach (var propValueGroup in store.Settings.GroupBy(x => x.Name))
				{
					var val = propValueGroup.Select(g => g.Value);
					if (val != null)
					{
						retVal.Settings.Add(propValueGroup.Key, val);
					}
				}
			}

			//if (store. != null)
			//{
			//	retVal.Seo = keywords.Select(x => x.ToWebModel()).ToArray();
			//}

			return retVal;
		}
	}
}