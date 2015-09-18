using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class ShippingMethodConverter
    {
        public static ShippingMethod ToViewModel(this DataContracts.Quotes.ShipmentMethod shipmentMethod)
        {
            var shippingMethodModel = new ShippingMethod();

            shippingMethodModel.Handle = shipmentMethod.ShipmentMethodCode;
            shippingMethodModel.Price = shipmentMethod.Price;

            return shippingMethodModel;
        }
    }
}