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

        public static VirtoCommerceCartModuleWebModelShoppingCart ToServiceModel(this ShoppingCart cart)
        {
            var cartServiceModel = new VirtoCommerceCartModuleWebModelShoppingCart();

            cartServiceModel.InjectFrom(cart);
            cartServiceModel.Currency = cart.Currency.CurrencyCode.ToString();
            cartServiceModel.DiscountTotal = (double)cart.DiscountTotal.Amount;
            cartServiceModel.HandlingTotal = (double)(cart.HandlingTotal != null ? cart.HandlingTotal.Amount : 0);
            cartServiceModel.ShippingTotal = (double)cart.ShippingTotal.Amount;
            cartServiceModel.SubTotal = (double)cart.SubTotal.Amount;
            cartServiceModel.TaxTotal = (double)cart.TaxTotal.Amount;
            cartServiceModel.Total = (double)cart.Total.Amount;

            if (cart.Coupon != null && cart.Coupon.AppliedSuccessfully)
            {
                cartServiceModel.Coupon = cart.Coupon.Code;
            }

            if (cart.Addresses != null)
            {
                cartServiceModel.Addresses = cart.Addresses.Select(a => a.ToServiceModel()).ToList();
            }

            if (cart.Discounts != null)
            {
                cartServiceModel.Discounts = cart.Discounts.Select(d => d.ToServiceModel()).ToList();
            }

            if (cart.Items != null)
            {
                cartServiceModel.Items = cart.Items.Select(i => i.ToServiceModel()).ToList();
            }

            if (cart.Payments != null)
            {
                cartServiceModel.Payments = cart.Payments.Select(i => i.ToServiceModel()).ToList();
            }

            if (cart.Shipments != null)
            {
                cartServiceModel.Shipments = cart.Shipments.Select(s => s.ToServiceModel()).ToList();
            }

            if (cart.TaxDetails != null)
            {
                cartServiceModel.TaxDetails = cart.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            }

            return cartServiceModel;
        }
    }
}