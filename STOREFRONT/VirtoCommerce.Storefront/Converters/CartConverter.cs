using Omu.ValueInjecter;
using System;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartConverter
    {
        public static ShoppingCart ToWebModel(this VirtoCommerceCartModuleWebModelShoppingCart serviceModel, Currency currency, Language language)
        {
            var webModel = new ShoppingCart(currency, language);

            webModel.InjectFrom(serviceModel);

            if (!string.IsNullOrEmpty(serviceModel.Coupon))
            {
                webModel.Coupon = new Coupon
                {
                    AppliedSuccessfully = true,
                    Code = serviceModel.Coupon
                };
            }

            if (serviceModel.Items != null)
            {
                webModel.Items = serviceModel.Items.Select(i => i.ToWebModel(currency, language)).ToList();
                webModel.HasPhysicalProducts = webModel.Items.Any(i =>
                    string.IsNullOrEmpty(i.ProductType) ||
                    !string.IsNullOrEmpty(i.ProductType) && i.ProductType.Equals("Physical", StringComparison.OrdinalIgnoreCase));
            }

            if (serviceModel.Addresses != null)
            {
                webModel.Addresses = serviceModel.Addresses.Select(a => a.ToWebModel()).ToList();

                var billingAddress = webModel.Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
                if (billingAddress == null)
                {
                    billingAddress = new Address { Type = AddressType.Billing };
                }

                if (webModel.HasPhysicalProducts)
                {
                    var shippingAddress = webModel.Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
                    if (shippingAddress == null)
                    {
                        shippingAddress = new Address { Type = AddressType.Shipping };
                    }
                }
            }

            if (serviceModel.Payments != null)
            {
                webModel.Payments = serviceModel.Payments.Select(p => p.TowebModel(currency)).ToList();
            }

            if (serviceModel.Shipments != null)
            {
                webModel.Shipments = serviceModel.Shipments.Select(s => s.ToWebModel(webModel)).ToList();
            }

            if (serviceModel.DynamicProperties != null)
            {
                webModel.DynamicProperties = serviceModel.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (serviceModel.TaxDetails != null)
            {
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel(currency)).ToList();
            }

            webModel.HandlingTotal = new Money(serviceModel.HandlingTotal ?? 0, currency);
            webModel.Height = (decimal)(serviceModel.Height ?? 0);
            webModel.IsAnonymous = serviceModel.IsAnonymous == true;
            webModel.IsRecuring = serviceModel.IsRecuring == true;
            webModel.Length = (decimal)(serviceModel.Length ?? 0);
            webModel.TaxIncluded = serviceModel.TaxIncluded == true;
            webModel.TaxTotal = new Money(serviceModel.TaxTotal ?? 0, currency);
            webModel.VolumetricWeight = (decimal)(serviceModel.VolumetricWeight ?? 0);
            webModel.Weight = (decimal)(serviceModel.Weight ?? 0);
            webModel.Width = (decimal)(serviceModel.Width ?? 0);
            webModel.ValidationType = EnumUtility.SafeParse(serviceModel.ValidationType, ValidationType.PriceAndQuantity);

            return webModel;
        }

        public static VirtoCommerceDomainTaxModelTaxEvaluationContext ToTaxEvalContext(this ShoppingCart cart)
        {
            var retVal = new VirtoCommerceDomainTaxModelTaxEvaluationContext();
            retVal.Id = cart.Id;
            retVal.Code = cart.Name;
            retVal.Currency = cart.Currency.Code;
            retVal.Type = "Cart";
            retVal.Lines = new System.Collections.Generic.List<VirtoCommerceDomainTaxModelTaxLine>();
            foreach (var lineItem in cart.Items)
            {
                var line = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = lineItem.Id,
                    Code = lineItem.Sku,
                    Name = lineItem.Name,
                    TaxType = lineItem.TaxType,
                    Amount = (double)lineItem.ExtendedPrice.Amount
                };
                retVal.Lines.Add(line);
            }
            foreach (var shipment in cart.Shipments)
            {
                var line = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = shipment.Id,
                    Code = shipment.ShipmentMethodCode,
                    Name = shipment.ShipmentMethodCode,
                    TaxType = shipment.TaxType,
                    Amount = (double)shipment.Total.Amount
                };
                retVal.Lines.Add(line);
            }
            return retVal;
        }

        public static VirtoCommerceCartModuleWebModelShoppingCart ToServiceModel(this ShoppingCart webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelShoppingCart();

            serviceModel.InjectFrom(webModel);

            serviceModel.Addresses = webModel.Addresses.Select(a => a.ToCartServiceModel()).ToList();
            serviceModel.Coupon = webModel.Coupon != null && webModel.Coupon.AppliedSuccessfully ? webModel.Coupon.Code : null;
            serviceModel.Currency = webModel.Currency.Code;
            serviceModel.Discounts = webModel.Discounts.Select(d => d.ToServiceModel()).ToList();
            serviceModel.DiscountTotal = (double)webModel.DiscountTotal.Amount;
            serviceModel.HandlingTotal = (double)webModel.HandlingTotal.Amount;
            serviceModel.Height = (double)webModel.Height;
            serviceModel.Items = webModel.Items.Select(i => i.ToServiceModel()).ToList();
            serviceModel.Length = (double)webModel.Length;
            serviceModel.Payments = webModel.Payments.Select(p => p.ToServiceModel()).ToList();
            serviceModel.Shipments = webModel.Shipments.Select(s => s.ToServiceModel()).ToList();
            serviceModel.ShippingTotal = (double)webModel.ShippingTotal.Amount;
            serviceModel.SubTotal = (double)webModel.SubTotal.Amount;
            serviceModel.TaxDetails = webModel.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            serviceModel.DynamicProperties = webModel.DynamicProperties.Select(dp => dp.ToServiceModel()).ToList();
            serviceModel.TaxTotal = (double)webModel.TaxTotal.Amount;
            serviceModel.Total = (double)webModel.Total.Amount;
            serviceModel.VolumetricWeight = (double)webModel.VolumetricWeight;
            serviceModel.Weight = (double)webModel.Weight;
            serviceModel.Width = (double)webModel.Width;
            serviceModel.ValidationType = webModel.ValidationType.ToString();

            return serviceModel;
        }
    }
}