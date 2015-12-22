using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class StoreConverter
    {
        public static Store ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceStoreModuleWebModelStore storeDto)
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
            retVal.DefaultCurrency = Currency.Get(EnumUtility.SafeParse(storeDto.DefaultCurrency, CurrencyCodes.USD));
            if(storeDto.Currencies != null)
            {
                retVal.Currencies = storeDto.Currencies.Select(x => Currency.Get(EnumUtility.SafeParse(x, CurrencyCodes.USD))).ToList();
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
