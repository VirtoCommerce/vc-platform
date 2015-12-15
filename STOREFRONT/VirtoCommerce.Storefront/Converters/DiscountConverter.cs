using Omu.ValueInjecter;
using System;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DiscountConverter
    {
        public static Discount ToWebModel(this VirtoCommerceCartModuleWebModelDiscount serviceModel)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);

            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, serviceModel.Currency);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelDiscount ToServiceModel(this Discount webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelDiscount();

            serviceModel.InjectFrom(webModel);

            serviceModel.Currency = webModel.Amount.CurrencyCode;
            serviceModel.DiscountAmount = (double)webModel.Amount.Amount;

            return serviceModel;
        }

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount serviceModel)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);

            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, serviceModel.Currency);

            return webModel;
        }

        public static Discount ToDiscountWebModel(this VirtoCommerceMarketingModuleWebModelPromotionReward reward, decimal amount, Currency currency)
        {
            var discount = new Discount();

            decimal absoluteDiscountAmount = 0;
            if (reward.AmountType.Equals("Absolute", StringComparison.OrdinalIgnoreCase))
            {
                absoluteDiscountAmount = (decimal)(reward.Amount ?? 0);
            }
            if (reward.AmountType.Equals("Relative", StringComparison.OrdinalIgnoreCase))
            {
                absoluteDiscountAmount = amount * (decimal)(reward.Amount ?? 0) / 100;
            }

            discount.Amount = new Money(absoluteDiscountAmount, currency.Code);
            discount.Description = reward.Promotion.Description;
            discount.PromotionId = reward.Promotion.Id;
            discount.Type = EnumUtility.SafeParse(reward.RewardType, PromotionRewardType.CatalogItemAmountReward);

            return discount;
        }
    }
}