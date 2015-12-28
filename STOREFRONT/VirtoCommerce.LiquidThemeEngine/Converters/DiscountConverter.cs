using VirtoCommerce.Storefront.Model.Marketing;
using ShopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class DiscountConverter
    {
        public static ShopifyModel.Discount ToShopifyModel(this Discount discount)
        {
            var result = new ShopifyModel.Discount
            {
                Amount = discount.Amount.Amount * 100,
                Code = discount.PromotionId,
                Id = discount.PromotionId,
                Savings = -discount.Amount.Amount * 100
            };

            return result;
        }
    }
}