using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PromotionProductEntryConverter
    {
        public static VirtoCommerceDomainMarketingModelProductPromoEntry ToServiceModel(this PromotionProductEntry webModel)
        {
            var serviceModel = new VirtoCommerceDomainMarketingModelProductPromoEntry();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Discount = webModel.Discount != null ? (double?)webModel.Discount.Amount : null;
            serviceModel.Price = webModel.Price != null ? (double?)webModel.Price.Amount : null;
            serviceModel.Variations = webModel.Variations != null ? webModel.Variations.Select(v => v.ToServiceModel()).ToList() : null;

            return serviceModel;
        }
    }
}