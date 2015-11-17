using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartConverter
    {
        public static ShoppingCart ToWebModel(this VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            var cartWebModel = new ShoppingCart();
            cartWebModel.InjectFrom(cart);

            cartWebModel.Currency = Currency.Get(EnumUtility.SafeParse(cart.Currency, CurrencyCodes.USD));

            return cartWebModel;
        }

        public static VirtoCommerceCartModuleWebModelShoppingCart ToServiceModel(this ShoppingCart cart)
        {
            var cartServiceModel = new VirtoCommerceCartModuleWebModelShoppingCart();

            cartServiceModel.InjectFrom(cart);

            return cartServiceModel;
        }
    }
}