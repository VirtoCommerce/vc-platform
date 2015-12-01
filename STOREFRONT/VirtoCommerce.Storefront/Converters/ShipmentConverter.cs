using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentConverter
    {
        public static Model.Cart.Shipment ToWebModel(this VirtoCommerceCartModuleWebModelShipment shipment)
        {
            var webModel = new Model.Cart.Shipment();

            webModel.InjectFrom(shipment);
            webModel.Currency = new Currency(EnumUtility.SafeParse(shipment.Currency, CurrencyCodes.USD));
            webModel.ShippingPrice = new Money(shipment.ShippingPrice ?? 0, shipment.Currency);

            if (shipment.DeliveryAddress != null)
            {
                webModel.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
            }

            if (shipment.Discounts != null)
            {
                webModel.Discounts = shipment.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (shipment.Items != null)
            {
                webModel.Items = shipment.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (shipment.TaxDetails != null)
            {
                webModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelShipment ToServiceModel(this Model.Cart.Shipment shipment)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelShipment();

            serviceModel.InjectFrom(shipment);
            serviceModel.Currency = shipment.Currency.Code;
            serviceModel.DiscountTotal = (double)shipment.DiscountTotal.Amount;
            serviceModel.TaxTotal = (double)shipment.TaxTotal.Amount;
            serviceModel.ShippingPrice = (double)shipment.ShippingPrice.Amount;
            serviceModel.Total = (double)shipment.Total.Amount;

            if (shipment.DeliveryAddress != null)
            {
                serviceModel.DeliveryAddress = shipment.DeliveryAddress.ToServiceModel();
            }

            if (shipment.Discounts != null)
            {
                serviceModel.Discounts = shipment.Discounts.Select(d => d.ToServiceModel()).ToList();
            }

            if (shipment.Items != null)
            {
                serviceModel.Items = shipment.Items.Select(i => i.ToServiceModel()).ToList();
            }

            if (shipment.TaxDetails != null)
            {
                serviceModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            }

            return serviceModel;
        }

        public static Model.Order.Shipment ToWebModel(this VirtoCommerceOrderModuleWebModelShipment shipment)
        {
            var webModel = new Model.Order.Shipment();

            var currency = new Currency(EnumUtility.SafeParse(shipment.Currency, CurrencyCodes.USD));

            webModel.InjectFrom(shipment);

            if (shipment.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = shipment.ChildrenOperations.Select(co => co.ToWebModel()).ToList();
            }

            webModel.Currency = currency;

            if (shipment.DeliveryAddress != null)
            {
                webModel.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
            }

            if (shipment.Discount != null)
            {
                webModel.Discount = shipment.Discount.ToWebModel();
            }

            webModel.DiscountAmount = new Money(shipment.DiscountAmount ?? 0, currency.Code);

            if (shipment.DynamicProperties != null)
            {
                webModel.DynamicProperties = shipment.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (shipment.InPayments != null)
            {
                webModel.InPayments = shipment.InPayments.Select(p => p.ToWebModel()).ToList();
            }

            if (shipment.Items != null)
            {
                webModel.Items = shipment.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (shipment.Packages != null)
            {
                webModel.Packages = shipment.Packages.Select(p => p.ToWebModel()).ToList();
            }

            webModel.Sum = new Money(shipment.Sum ?? 0, currency.Code);
            webModel.Tax = new Money(shipment.Tax ?? 0, currency.Code);

            if (shipment.TaxDetails != null)
            {
                webModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}