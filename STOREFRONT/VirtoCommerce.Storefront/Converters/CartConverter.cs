using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartConverter
    {
        public static ShoppingCart ToWebModel(this VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            var cartWebModel = new ShoppingCart(cart.StoreId, cart.CustomerId, cart.CustomerName, cart.Name, cart.Currency);

            var currency = new Currency(EnumUtility.SafeParse(cart.Currency, CurrencyCodes.USD));

            cartWebModel.InjectFrom(cart);

            cartWebModel.HandlingTotal = new Money(cart.HandlingTotal ?? 0, currency.Code);

            if (cart.Addresses != null)
            {
                cartWebModel.Addresses = cart.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (cart.Discounts != null)
            {
                cartWebModel.Discounts = cart.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (cart.Items != null)
            {
                cartWebModel.Items = cart.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (cart.Payments != null)
            {
                cartWebModel.Payments = cart.Payments.Select(p => p.TowebModel()).ToList();
            }

            if (cart.Shipments != null)
            {
                cartWebModel.Shipments = cart.Shipments.Select(s => s.ToWebModel()).ToList();
            }

            if (cart.TaxDetails != null)
            {
                cartWebModel.TaxDetails = cart.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return cartWebModel;
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