using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class StoreConverter
    {
        public static Store ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceStoreModuleWebModelStore storeDto, IEnumerable<Currency> availCurrencies)
        {
            var retVal = new Store();
            retVal.InjectFrom(storeDto);
            if(storeDto.SeoInfos != null)
            {
                retVal.SeoInfos = storeDto.SeoInfos.Select(x => x.ToWebModel()).ToList();
            }
            retVal.DefaultLanguage = storeDto.DefaultLanguage != null ? new Language(storeDto.DefaultLanguage) : new Language("en-US");
            if(storeDto.Languages != null)
            {
                retVal.Languages = storeDto.Languages.Select(x => new Language(x)).ToList();
            }
            retVal.DefaultCurrency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(storeDto.DefaultCurrency)) ?? new Currency(storeDto.DefaultCurrency, 1);

            if (storeDto.Currencies != null)
            {
                retVal.Currencies = availCurrencies.Where(x => x.IsHasSameCode(storeDto.DefaultCurrency) || storeDto.Currencies.Any(y => x.IsHasSameCode(y))).ToList();
            }
            if(storeDto.DynamicProperties != null)
            {
                retVal.DynamicProperties = storeDto.DynamicProperties.Select(x => x.ToWebModel()).ToList();
                retVal.ThemeName = retVal.DynamicProperties.GetDynamicPropertyValue("DefaultThemeName");
            }
            
        
            return retVal;
        }
    }
}
