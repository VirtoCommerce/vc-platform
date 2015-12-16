using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PromotionEvaluationContextConverter
    {
        public static VirtoCommerceDomainMarketingModelPromotionEvaluationContext ToServiceModel(this PromotionEvaluationContext webModel)
        {
            var serviceModel = new VirtoCommerceDomainMarketingModelPromotionEvaluationContext();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.CartPromoEntries = webModel.CartPromoEntries.Select(pe => pe.ToServiceModel()).ToList();
            serviceModel.CartTotal = webModel.CartTotal != null ? (double?)webModel.CartTotal.Amount : null;
            serviceModel.Currency = webModel.Currency != null ? webModel.Currency.Code : null;
            serviceModel.Language = webModel.Language != null ? webModel.Language.CultureName : null;
            serviceModel.PromoEntries = webModel.PromoEntries.Select(pe => pe.ToServiceModel()).ToList();
            serviceModel.PromoEntry = webModel.PromoEntry != null ? webModel.PromoEntry.ToServiceModel() : null;
            serviceModel.ShipmentMethodPrice = webModel.ShipmentMethodPrice != null ? (double?)webModel.ShipmentMethodPrice.Amount : null;

            return serviceModel;
        }
    }
}