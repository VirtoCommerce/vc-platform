using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentConverter
    {
        public static Shipment ToWebModel(this VirtoCommerceCartModuleWebModelShipment shipment)
        {
            var shipmentWebModel = new Shipment();

            shipmentWebModel.InjectFrom(shipment);
            shipmentWebModel.Currency = new Currency(EnumUtility.SafeParse(shipment.Currency, CurrencyCodes.USD));

            if (shipment.DeliveryAddress != null)
            {
                shipmentWebModel.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
            }

            if (shipment.Discounts != null)
            {
                shipmentWebModel.Discounts = shipment.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (shipment.Items != null)
            {
                shipmentWebModel.Items = shipment.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (shipment.TaxDetails != null)
            {
                shipmentWebModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return shipmentWebModel;
        }
    }
}