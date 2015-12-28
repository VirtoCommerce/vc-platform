using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentPackageConverter
    {
        public static ShipmentPackage ToWebModel(this VirtoCommerceOrderModuleWebModelShipmentPackage shipmentPackage)
        {
            var webModel = new ShipmentPackage();

            webModel.InjectFrom(shipmentPackage);

            if (shipmentPackage.Items != null)
            {
                webModel.Items = shipmentPackage.Items.Select(i => i.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}