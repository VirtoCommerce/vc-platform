using System.Linq;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public interface IFoundationMarketingRepository : IMarketingRepository
	{
		Promotion GetPromotionById(string id);
		Promotion[] GetActivePromotions();
	}
}
