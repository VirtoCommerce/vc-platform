using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DynamicContentConverter
    {
        public static DynamicContentItem ToWebModel(this VirtoCommerceMarketingModuleWebModelDynamicContentItem serviceModel)
        {
            var webModel = new DynamicContentItem();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            if (serviceModel.DynamicProperties != null)
            {
                webModel.DynamicProperties = serviceModel.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}