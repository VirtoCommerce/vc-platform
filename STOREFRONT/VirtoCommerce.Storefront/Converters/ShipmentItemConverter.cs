using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentItemConverter
    {
        public static ShipmentItem ToWebModel(this VirtoCommerceOrderModuleWebModelShipmentItem shipmentItem)
        {
            var webModel = new ShipmentItem();

            webModel.InjectFrom(shipmentItem);

            if (shipmentItem.LineItem != null)
            {
                webModel.LineItem = shipmentItem.LineItem.ToWebModel();
            }

            return webModel;
        }
    }
}