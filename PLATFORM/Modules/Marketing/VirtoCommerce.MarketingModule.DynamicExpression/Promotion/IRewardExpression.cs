using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.DynamicExpression.Promotion
{
	public interface IRewardExpression
	{
		PromotionReward[] GetRewards();
	}
}