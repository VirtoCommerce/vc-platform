using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.Scheduling.Jobs
{
    /// <summary>
    /// Processes (update) newly created order statuses.
    /// </summary>
	public class RemoveExpiredPromotionUsagesWork : IJobActivity
	{
		private readonly IMarketingRepository _marketingRepository;
		public RemoveExpiredPromotionUsagesWork(IMarketingRepository marketingRepository)
		{
			_marketingRepository = marketingRepository;
		}

		public void Execute(IJobContext context)
		{
			var currentTime = DateTime.UtcNow;
			var statusFilter = (int)PromotionUsageStatus.Reserved;
			var timeout = context.Parameters != null && context.Parameters.ContainsKey("expirationtimeout")
			? int.Parse(context.Parameters["expirationtimeout"]) : -1;

			if (timeout >= 0)
				using (_marketingRepository)
				{
					var expiredPromotions = _marketingRepository.PromotionUsages.Where(promotion => promotion.Status == statusFilter && promotion.UsageDate != null && promotion.UsageDate < currentTime.AddMinutes(-timeout)).ToList();

					foreach (var item in expiredPromotions)
					{
						_marketingRepository.Remove(item);
						_marketingRepository.UnitOfWork.Commit();
					}
				}
		}
	}
}
