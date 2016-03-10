using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class SettingConverter
    {
        public static SettingEntry ToWebModel(this VirtoCommerceStoreModuleWebModelSetting dto)
        {
            var retVal = new SettingEntry();
            retVal.InjectFrom<NullableAndEnumValueInjecter>(dto);
            retVal.AllowedValues = dto.AllowedValues;
            retVal.ArrayValues = dto.ArrayValues;
            return retVal;
        }
    }
}