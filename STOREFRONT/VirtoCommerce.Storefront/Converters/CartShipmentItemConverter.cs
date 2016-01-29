using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartShipmentItemConverter
    {
        public static CartShipmentItem ToWebModel(this VirtoCommerceCartModuleWebModelShipmentItem serviceModel, ShoppingCart cart)
        {
            var webModel = new CartShipmentItem();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.LineItem = cart.Items.FirstOrDefault(x => x.Id == serviceModel.LineItemId);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelShipmentItem ToServiceModel(this CartShipmentItem webModel)
        {
            var result = new VirtoCommerceCartModuleWebModelShipmentItem();

            result.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            result.LineItem = webModel.LineItem.ToServiceModel();

            return result;
        }
    }
}