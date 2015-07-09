using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class DictionaryItemConverter
    {
        public static DictionaryItem ToWebModel(this DynamicPropertyDictionaryItem model)
        {
            var result = new DictionaryItem();
            result.InjectFrom(model);

            if (model.DictionaryValues != null)
            {
                result.DictionaryValues = model.DictionaryValues.Select(x => x.ToWebModel()).ToArray();
            }

            return result;
        }

        public static DynamicPropertyDictionaryItem ToCoreModel(this  DictionaryItem model)
        {
            var result = new DynamicPropertyDictionaryItem();
            result.InjectFrom(model);

            if (model.DictionaryValues != null)
            {
                result.DictionaryValues = model.DictionaryValues.Select(x => x.ToCoreModel()).ToArray();
            }

            return result;
        }
    }
}
