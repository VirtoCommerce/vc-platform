using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public interface IPromotionUsageProvider
	{
		int GetTotalUsageCount(string promotionId);
		int GetUsagePerCustomerCount(string promotionId, string customerId);
	}
}
