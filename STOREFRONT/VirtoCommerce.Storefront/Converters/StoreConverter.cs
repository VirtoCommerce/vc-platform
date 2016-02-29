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
            if(!storeDto.SeoInfos.IsNullOrEmpty())
            {
                retVal.SeoInfos = storeDto.SeoInfos.Select(x => x.ToWebModel()).ToList();
            }
            retVal.DefaultLanguage = storeDto.DefaultLanguage != null ? new Language(storeDto.DefaultLanguage) : Language.InvariantLanguage;
            if(!storeDto.Languages.IsNullOrEmpty())
            {
                retVal.Languages = storeDto.Languages.Select(x => new Language(x)).ToList();
            }

            if (!storeDto.Currencies.IsNullOrEmpty())
            {
                retVal.Currencies.AddRange(storeDto.Currencies.Select(x => new Currency(Language.InvariantLanguage, x)));
            }
            retVal.DefaultCurrency = retVal.Currencies.FirstOrDefault(x => x.Equals(storeDto.DefaultCurrency));

            if (!storeDto.DynamicProperties.IsNullOrEmpty())
            {
                retVal.DynamicProperties = storeDto.DynamicProperties.Select(x => x.ToWebModel()).ToList();
                retVal.ThemeName = retVal.DynamicProperties.GetDynamicPropertyValue("DefaultThemeName");
            }

            retVal.TrustedGroups = storeDto.TrustedGroups;
            retVal.StoreState = EnumUtility.SafeParse(storeDto.StoreState, StoreStatus.Open);

            return retVal;
        }
    }
}