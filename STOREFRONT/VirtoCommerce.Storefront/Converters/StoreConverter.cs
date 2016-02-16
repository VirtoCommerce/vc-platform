using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Platform.Core.Common;

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
            retVal.DefaultLanguage = storeDto.DefaultLanguage != null ? new Language(storeDto.DefaultLanguage) : Language.InvariantLanguage;
            if(storeDto.Languages != null)
            {
                retVal.Languages = storeDto.Languages.Select(x => new Language(x)).ToList();
            }

            if (storeDto.Currencies != null)
            {
                retVal.Currencies.AddRange(storeDto.Currencies.Select(x => new Currency(Language.InvariantLanguage, x)));
            }
            retVal.DefaultCurrency = retVal.Currencies.FirstOrDefault(x => x.Equals(storeDto.DefaultCurrency));

            if (storeDto.DynamicProperties != null)
            {
                retVal.DynamicProperties = storeDto.DynamicProperties.Select(x => x.ToWebModel()).ToList();
                retVal.ThemeName = retVal.DynamicProperties.GetDynamicPropertyValue("DefaultThemeName");
            }

            return retVal;
        }
    }
}