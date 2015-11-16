using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartLineItemConverter
    {
        public static LineItem ToWebModel(this VirtoCommerceCartModuleWebModelLineItem lineItem)
        {
            var lineItemWebModel = new LineItem();

            lineItemWebModel.InjectFrom(lineItem);

            return lineItemWebModel;
        }

        public static VirtoCommerceCartModuleWebModelLineItem ToServiceModel(this LineItem lineItem)
        {
            var lineItemServiceModel = new VirtoCommerceCartModuleWebModelLineItem();

            lineItemServiceModel.InjectFrom(lineItem);

            return lineItemServiceModel;
        }
    }
}