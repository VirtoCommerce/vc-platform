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
            var webModel = new Discount();

            webModel.InjectFrom(discount);

            webModel.AbsoluteAmount = new Money(discount.DiscountAmount ?? 0, discount.Currency);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelDiscount ToServiceModel(this Discount discount)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelDiscount();

            serviceModel.InjectFrom(discount);

            serviceModel.Currency = discount.AbsoluteAmount.CurrencyCode;
            serviceModel.DiscountAmount = (double)discount.AbsoluteAmount.Amount;

            return serviceModel;
        }

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount discount)
        {
            var webModel = new Discount();

            webModel.InjectFrom(discount);

            webModel.AbsoluteAmount = new Money(discount.DiscountAmount ?? 0, discount.Currency);

            return webModel;
        }

        public static Discount ToDiscountWebModel(this VirtoCommerceMarketingModuleWebModelPromotionReward promotionReward, Currency currency)
        {
            var discountModel = new Discount();

            discountModel.InjectFrom(promotionReward);

            switch (promotionReward.AmountType)
            {
                case "Absolute":
                    discountModel.AbsoluteAmount = new Money(promotionReward.Amount ?? 0, currency.Code);
                    discountModel.Type = AmountType.Absolute;
                    break;
                case "Relative":
                    discountModel.AbsoluteAmount = new Money(0, currency.Code);
                    discountModel.RelativeAmount = promotionReward.Amount;
                    discountModel.Type = AmountType.Relative;
                    break;
            }

            discountModel.PromotionId = promotionReward.Promotion.Id;

            return discountModel;
        }
    }
}