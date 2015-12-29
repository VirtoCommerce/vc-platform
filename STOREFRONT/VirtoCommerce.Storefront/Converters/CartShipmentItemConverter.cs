using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Cart;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartShipmentItemConverter
    {
        public static CartShipmentItem ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceCartModuleWebModelShipmentItem shipmentItemDto, ShoppingCart cart)
        {
            var retVal = new CartShipmentItem();
            retVal.InjectFrom(shipmentItemDto);
            retVal.LineItem = cart.Items.FirstOrDefault(x => x.Id == shipmentItemDto.LineItemId);

            return retVal;
        }

        public static VirtoCommerce.Client.Model.VirtoCommerceCartModuleWebModelShipmentItem ToServiceModel(this CartShipmentItem shipmentItem)
        {
            var retVal = new VirtoCommerce.Client.Model.VirtoCommerceCartModuleWebModelShipmentItem();
            retVal.LineItem = shipmentItem.LineItem.ToServiceModel();
            return retVal;
        }
    }
}
