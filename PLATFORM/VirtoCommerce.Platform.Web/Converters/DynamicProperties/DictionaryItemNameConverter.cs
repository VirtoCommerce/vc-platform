using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class DictionaryItemNameConverter
    {
        public static DictionaryItemName ToWebModel(this DynamicPropertyDictionaryItemName coreModel)
        {
            var result = new DictionaryItemName();
            result.InjectFrom(coreModel);
            return result;
        }

        public static DynamicPropertyDictionaryItemName ToCoreModel(this DictionaryItemName webModel)
        {
            var result = new DynamicPropertyDictionaryItemName();
            result.InjectFrom(webModel);
            return result;
        }
    }
}
