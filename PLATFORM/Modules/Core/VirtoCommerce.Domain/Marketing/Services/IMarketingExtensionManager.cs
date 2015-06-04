using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IMarketingExtensionManager
	{
		PromoDynamicExpressionTree PromotionDynamicExpressionTree { get; set; }
		ConditionExpressionTree DynamicContentExpressionTree { get; set; }

		void RegisterPromotion(Promotion promotion);
		IEnumerable<Promotion> Promotions { get; }
	}
}
