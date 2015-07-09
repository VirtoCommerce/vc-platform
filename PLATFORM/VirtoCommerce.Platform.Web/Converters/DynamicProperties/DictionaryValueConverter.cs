using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class DictionaryValueConverter
    {
        public static DictionaryValue ToWebModel(this DynamicPropertyDictionaryValue model)
        {
            var result = new DictionaryValue();
            result.InjectFrom(model);
            return result;
        }

        public static DynamicPropertyDictionaryValue ToCoreModel(this  DictionaryValue model)
        {
            var result = new DynamicPropertyDictionaryValue();
            result.InjectFrom(model);
            return result;
        }
    }
}
