using System.Collections.Generic;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentItemConverter
    {
        public static ShipmentItem ToWebModel(this VirtoCommerceOrderModuleWebModelShipmentItem shipmentItem, IEnumerable<Currency> availCurrencies, Language language)
        {
            var webModel = new ShipmentItem();

            webModel.InjectFrom(shipmentItem);

            if (shipmentItem.LineItem != null)
            {
                webModel.LineItem = shipmentItem.LineItem.ToWebModel(availCurrencies, language);
            }

            return webModel;
        }
    }
}