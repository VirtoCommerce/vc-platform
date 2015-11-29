using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DiscountConverter
    {
        public static Discount ToWebModel(this VirtoCommerceCartModuleWebModelDiscount discount)
        {
            var discountWebModel = new Discount();

            discountWebModel.InjectFrom(discount);

            return discountWebModel;
        }

        public static VirtoCommerceCartModuleWebModelDiscount ToServiceModel(this Discount discount)
        {
            var discountServiceModel = new VirtoCommerceCartModuleWebModelDiscount();

            discountServiceModel.InjectFrom(discount);

            return discountServiceModel;
        }

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount discount, Currency currency)
        {
            var webModel = new Discount();

            webModel.InjectFrom(discount);

            webModel.DiscountAmount = new Money(discount.DiscountAmount ?? 0, currency.Code);

            return webModel;
        }
    }
}