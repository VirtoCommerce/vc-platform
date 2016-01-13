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

            webModel.Amount = (decimal)(serviceModel.Amount ?? 0);
            webModel.AmountType = EnumUtility.SafeParse(serviceModel.AmountType, AmountType.Absolute);
            webModel.CouponAmount = new Money(serviceModel.CouponAmount ?? 0, currency);
            webModel.CouponMinOrderAmount = new Money(serviceModel.CouponMinOrderAmount ?? 0, currency);
            webModel.Promotion = serviceModel.Promotion.ToWebModel();
            webModel.RewardType = EnumUtility.SafeParse(serviceModel.RewardType, PromotionRewardType.CatalogItemAmountReward);
            webModel.ShippingMethodCode = serviceModel.ShippingMethod;

            return webModel;
        }
    }
}