using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface IMarketingService
    {
        Task<ICollection<PromotionReward>> EvaluatePromotionRewardsAsync(PromotionEvaluationContext evaluationContext);

        Task<string> GetDynamicContentHtmlAsync(string storeId, string placeholderName);
    }
}