using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model.Store;
using foundation = VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class StoreConverter
    {
        public static webModel.Store ToWebModel(this foundation.Store store, Foundation.AppConfig.Model.SeoUrlKeyword[] keywords)
        {
            var retVal = new webModel.Store
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
                retVal.Settings = store.Settings.Select(x => x.ToWebModel()).ToArray();
            }

            if (keywords != null)
            {
                retVal.SeoKeywords = keywords.Select(x => x.ToWebModel()).ToArray();
            }

            return retVal;
        }

        public static webModel.StoreSetting ToWebModel(this foundation.StoreSetting setting)
        {
            var retVal = new webModel.StoreSetting();
            retVal.InjectFrom(setting);
            retVal.Value = setting.ToString();
            return retVal;
        }

    }
}
