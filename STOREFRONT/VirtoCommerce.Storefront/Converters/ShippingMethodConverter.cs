using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShippingMethodConverter
    {
        public static ShippingMethod ToWebModel(this VirtoCommerceQuoteModuleWebModelShipmentMethod serviceModel, Currency currency)
        {
            var webModel = new ShippingMethod();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.Price = new Money(serviceModel.Price ?? 0, currency);

            return webModel;
        }
    }
}
