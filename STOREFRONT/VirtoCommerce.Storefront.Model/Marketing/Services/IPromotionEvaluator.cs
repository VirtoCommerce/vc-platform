using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Marketing.Services
{
    public interface IPromotionEvaluator
    {
        Task EvaluateDiscountsAsync(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners);
        void EvaluateDiscounts(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners);
    }
}