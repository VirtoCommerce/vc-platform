using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.MerchandisingModule.Web.Model.Stores;
using foundation = VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class StoreConverter
    {
        #region Public Methods and Operators

        public static Store ToWebModel(this foundation.Store store, SeoUrlKeyword[] keywords)
        {
            var retVal = new Store
                         {
                             Id = store.StoreId
                         };

            retVal.InjectFrom(store);

            if (store.Languages != null)
            {
                retVal.Languages = store.Languages.Select(x => x.LanguageCode).ToArray();
            }

            if (store.Currencies != null)
            {
                retVal.Currencies = store.Currencies.Select(x => x.CurrencyCode).ToArray();
            }

            if (store.LinkedStores != null)
            {
                retVal.LinkedStores = store.LinkedStores.Select(x => x.LinkedStoreId).ToArray();
            }

            if (store.Settings != null)
            {
                retVal.Settings = new PropertyDictionary();

                foreach (var propValueGroup in store.Settings.GroupBy(x => x.Name))
                {
                    var val = propValueGroup.Select(g => g.RawValue());
                    if (val.Any())
                    {
                        retVal.Settings.Add(propValueGroup.Key, val.Count() > 1 ? val : val.First());
                    }
                }
            }

            if (keywords != null)
            {
                retVal.Seo = keywords.Select(x => x.ToWebModel()).ToArray();
            }

            return retVal;
        }
        #endregion
    }
}
