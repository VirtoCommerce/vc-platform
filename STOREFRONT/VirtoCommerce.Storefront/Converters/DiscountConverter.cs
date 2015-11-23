using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

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
    }
}