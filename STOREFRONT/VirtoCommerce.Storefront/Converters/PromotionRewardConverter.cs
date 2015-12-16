using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PromotionRewardConverter
    {
        public static PromotionReward ToWebModel(this VirtoCommerceMarketingModuleWebModelPromotionReward serviceModel, Currency currency)
        {
            var webModel = new PromotionReward();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.Amount = new Money(serviceModel.Amount ?? 0, currency.Code);
            webModel.AmountType = EnumUtility.SafeParse(serviceModel.AmountType, AmountType.Absolute);
            webModel.CouponAmount = new Money(serviceModel.CouponAmount ?? 0, currency.Code);
            webModel.CouponMinOrderAmount = new Money(serviceModel.CouponMinOrderAmount ?? 0, currency.Code);
            webModel.Promotion = serviceModel.Promotion.ToWebModel();
            webModel.RewardType = EnumUtility.SafeParse(serviceModel.RewardType, PromotionRewardType.CatalogItemAmountReward);

            return webModel;
        }

        public static Discount ToDiscountWebModel(this PromotionReward reward, decimal amount, Currency currency)
        {
            var discount = new Discount();

            decimal absoluteDiscountAmount = 0;
            if (reward.AmountType == AmountType.Absolute)
            {
                absoluteDiscountAmount = reward.Amount.Amount;
            }
            if (reward.AmountType == AmountType.Relative)
            {
                absoluteDiscountAmount = amount * reward.Amount.Amount / 100;
            }

            discount.Amount = new Money(absoluteDiscountAmount, currency.Code);
            discount.Description = reward.Promotion.Description;
            discount.PromotionId = reward.Promotion.Id;
            discount.Type = reward.RewardType;

            return discount;
        }
    }
}