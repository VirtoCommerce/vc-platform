using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Converters.DynamicProperties
{
    public static class DictionaryItemConverter
    {
        public static DictionaryItem ToWebModel(this DynamicPropertyDictionaryItem coreModel)
        {
            var result = new DictionaryItem();
            result.InjectFrom(coreModel);

            if (coreModel.DisplayNames != null)
            {
                result.DisplayNames = coreModel.DisplayNames.Select(x => x.ToWebModel()).ToArray();
            }

            return result;
        }

        public static DynamicPropertyDictionaryItem ToCoreModel(this DictionaryItem webModel)
        {
            var result = new DynamicPropertyDictionaryItem();
            result.InjectFrom(webModel);

            if (webModel.DisplayNames != null)
            {
                result.DisplayNames = webModel.DisplayNames.Select(x => x.ToCoreModel()).ToArray();
            }

            return result;
        }
    }
}
