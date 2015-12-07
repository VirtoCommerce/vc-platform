using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartConverter
    {
        public static ShoppingCart ToWebModel(this VirtoCommerceCartModuleWebModelShoppingCart serviceModel)
        {
            var webModel = new ShoppingCart(serviceModel.StoreId, serviceModel.CustomerId, serviceModel.CustomerName, serviceModel.Name, serviceModel.Currency);

            var currency = new Currency(EnumUtility.SafeParse(serviceModel.Currency, CurrencyCodes.USD));

            webModel.InjectFrom(serviceModel);

            webModel.HandlingTotal = new Money(serviceModel.HandlingTotal ?? 0, currency.Code);

            if (serviceModel.Addresses != null)
            {
                webModel.Addresses = serviceModel.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (serviceModel.Discounts != null)
            {
                webModel.Discounts = serviceModel.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (serviceModel.Items != null)
            {
                webModel.Items = serviceModel.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (serviceModel.Payments != null)
            {
                webModel.Payments = serviceModel.Payments.Select(p => p.TowebModel()).ToList();
            }

            if (serviceModel.Shipments != null)
            {
                webModel.Shipments = serviceModel.Shipments.Select(s => s.ToWebModel()).ToList();
            }

            if (serviceModel.TaxDetails != null)
            {
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            if (!string.IsNullOrEmpty(serviceModel.Coupon))
            {
                webModel.Coupon = new Coupon
                {
                    AppliedSuccessfully = true,
                    Code = serviceModel.Coupon
                };
            }

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelShoppingCart ToServiceModel(this ShoppingCart webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelShoppingCart();

            serviceModel.InjectFrom(webModel);
            serviceModel.Currency = webModel.Currency.CurrencyCode.ToString();
            serviceModel.DiscountTotal = (double)webModel.DiscountTotal.Amount;
            serviceModel.HandlingTotal = (double)(webModel.HandlingTotal != null ? webModel.HandlingTotal.Amount : 0);
            serviceModel.ShippingTotal = (double)webModel.ShippingTotal.Amount;
            serviceModel.SubTotal = (double)webModel.SubTotal.Amount;
            serviceModel.TaxTotal = (double)webModel.TaxTotal.Amount;
            serviceModel.Total = (double)webModel.Total.Amount;

            if (webModel.Coupon != null && webModel.Coupon.AppliedSuccessfully)
            {
                serviceModel.Coupon = webModel.Coupon.Code;
            }

            if (webModel.Addresses != null)
            {
                serviceModel.Addresses = webModel.Addresses.Select(a => a.ToServiceModel()).ToList();
            }

            if (webModel.Discounts != null)
            {
                serviceModel.Discounts = webModel.Discounts.Select(d => d.ToServiceModel()).ToList();
            }

            if (webModel.Items != null)
            {
                serviceModel.Items = webModel.Items.Select(i => i.ToServiceModel()).ToList();
            }

            if (webModel.Payments != null)
            {
                serviceModel.Payments = webModel.Payments.Select(i => i.ToServiceModel()).ToList();
            }

            if (webModel.Shipments != null)
            {
                serviceModel.Shipments = webModel.Shipments.Select(s => s.ToServiceModel()).ToList();
            }

            if (webModel.TaxDetails != null)
            {
                serviceModel.TaxDetails = webModel.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            }

            return serviceModel;
        }
    }
}