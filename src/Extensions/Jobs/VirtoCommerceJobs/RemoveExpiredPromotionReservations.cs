using System;
using System.Linq;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.Scheduling.Jobs
{
    /// <summary>
    /// Processes removes reserved promotion usages after predifened expirationtimeout
    /// </summary>
	public class RemoveExpiredPromotionReservations : IJobActivity
	{
		private readonly IMarketingRepository _marketingRepository;
        public RemoveExpiredPromotionReservations(IMarketingRepository marketingRepository)
		{
			_marketingRepository = marketingRepository;
		}

		public void Execute(IJobContext context)
		{
		    int parsedTimeout;
			var timeout = context.Parameters != null
                && context.Parameters.ContainsKey("expirationtimeout") 
                && int.TryParse(context.Parameters["expirationtimeout"], out parsedTimeout) 
                && parsedTimeout > 0 ? parsedTimeout  : 5; //Default timeout 5mins
            
		        using (_marketingRepository)
		        {
                    var expiredTime =  DateTime.UtcNow.AddMinutes(-timeout);
		            var expiredPromotions =_marketingRepository.PromotionUsages.Where(p =>
		                        p.Status == (int) PromotionUsageStatus.Reserved && p.UsageDate.HasValue &&
		                        p.UsageDate.Value < expiredTime).ToList();

		            if (expiredPromotions.Any())
		            {
		                foreach (var item in expiredPromotions)
		                {
		                    //item.Status = (int)PromotionUsageStatus.Expired;
                            _marketingRepository.Remove(item);
		                }
		                _marketingRepository.UnitOfWork.Commit();
		            }
		        }

		}
	}
}
