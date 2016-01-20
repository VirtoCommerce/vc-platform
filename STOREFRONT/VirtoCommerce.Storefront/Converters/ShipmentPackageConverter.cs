using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentPackageConverter
    {
        public static ShipmentPackage ToWebModel(this VirtoCommerceOrderModuleWebModelShipmentPackage shipmentPackage, IEnumerable<Currency> currencies, Language language)
        {
            var webModel = new ShipmentPackage();

            webModel.InjectFrom(shipmentPackage);

            if (shipmentPackage.Items != null)
            {
                webModel.Items = shipmentPackage.Items.Select(i => i.ToWebModel(currencies, language)).ToList();
            }

            return webModel;
        }
    }
}