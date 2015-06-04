using System.Linq;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public class PromotionUsageProvider : IPromotionUsageProvider
	{
        private readonly IMarketingRepository _repository;
		public PromotionUsageProvider(IMarketingRepository repository)
		{
			_repository = repository;
		}
		#region IPromotionUsageProvider Members

		public int GetTotalUsageCount(string promotionId)
		{
			var retVal = _repository.PromotionUsages.Count(x => x.PromotionId == promotionId && x.Status != (int)PromotionUsageStatus.Expired);
			return retVal;
		}

		public int GetUsagePerCustomerCount(string promotionId, string customerId)
		{
            var retVal = _repository.PromotionUsages.Count(x => x.PromotionId == promotionId && x.MemberId == customerId && x.Status != (int)PromotionUsageStatus.Expired);
			return retVal;
		}

		#endregion
	}
}
