using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ShipmentConverter
    {
        public static Model.Cart.Shipment ToWebModel(this VirtoCommerceCartModuleWebModelShipment shipment, ShoppingCart cart)
        {
            var webModel = new Model.Cart.Shipment(cart.Currency);

            webModel.InjectFrom(shipment);
            webModel.Currency = cart.Currency;
            webModel.ShippingPrice = new Money(shipment.ShippingPrice ?? 0, cart.Currency);
            webModel.TaxTotal = new Money(shipment.TaxTotal ?? 0, cart.Currency);

            if (shipment.DeliveryAddress != null)
            {
                webModel.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
            }

            if (shipment.Items != null)
            {
                webModel.Items = shipment.Items.Select(i => i.ToWebModel(cart)).ToList();
            }

            if (shipment.TaxDetails != null)
            {
                webModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToWebModel(cart.Currency)).ToList();
            }

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelShipment ToServiceModel(this Model.Cart.Shipment shipment)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelShipment();

            serviceModel.InjectFrom(shipment);
            serviceModel.Currency = shipment.Currency.Code;
            serviceModel.DiscountTotal = shipment.DiscountTotal != null ? (double?)shipment.DiscountTotal.Amount : null;
            serviceModel.TaxTotal = shipment.TaxTotal != null ? (double?)shipment.TaxTotal.Amount : null;
            serviceModel.ShippingPrice = (double)shipment.ShippingPrice.Amount;
            serviceModel.Total = (double)shipment.Total.Amount;

            if (shipment.DeliveryAddress != null)
            {
                serviceModel.DeliveryAddress = shipment.DeliveryAddress.ToCartServiceModel();
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

        public static Model.Order.Shipment ToWebModel(this VirtoCommerceOrderModuleWebModelShipment shipment, IEnumerable<Currency> availCurrencies, Language language)
        {
            var webModel = new Model.Order.Shipment();

            var currency = availCurrencies.FirstOrDefault(x => x.Equals(shipment.Currency)) ?? new Currency(language, shipment.Currency);

            webModel.InjectFrom(shipment);

            if (shipment.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = shipment.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies, language)).ToList();
            }

            webModel.Currency = currency;

            if (shipment.DeliveryAddress != null)
            {
                webModel.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
            }

            if (shipment.Discount != null)
            {
                webModel.Discount = shipment.Discount.ToWebModel(availCurrencies, language);
            }

            webModel.DiscountAmount = new Money(shipment.DiscountAmount ?? 0, currency);

            if (shipment.DynamicProperties != null)
            {
                webModel.DynamicProperties = shipment.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (shipment.InPayments != null)
            {
                webModel.InPayments = shipment.InPayments.Select(p => p.ToWebModel(availCurrencies, language)).ToList();
            }

            if (shipment.Items != null)
            {
                webModel.Items = shipment.Items.Select(i => i.ToWebModel(availCurrencies, language)).ToList();
            }

            if (shipment.Packages != null)
            {
                webModel.Packages = shipment.Packages.Select(p => p.ToWebModel(availCurrencies, language)).ToList();
            }

            webModel.Sum = new Money(shipment.Sum ?? 0, currency);
            webModel.Tax = new Money(shipment.Tax ?? 0, currency);

            if (shipment.TaxDetails != null)
            {
                webModel.TaxDetails = shipment.TaxDetails.Select(td => td.ToWebModel(currency)).ToList();
            }

            return webModel;
        }

        public static Shipment ToShipmentModel(this ShipmentUpdateModel updateModel, Currency currency)
        {
            var shipmentModel = new Shipment(currency);

            shipmentModel.InjectFrom<NullableAndEnumValueInjecter>(updateModel);

            return shipmentModel;
        }
    }
}