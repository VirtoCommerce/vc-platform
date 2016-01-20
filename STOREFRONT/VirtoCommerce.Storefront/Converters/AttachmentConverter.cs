using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class AttachmentConverter
    {
        public static Attachment ToWebModel(this VirtoCommerceQuoteModuleWebModelQuoteAttachment serviceModel)
        {
            var webModel = new Attachment();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            return webModel;
        }
    }
}