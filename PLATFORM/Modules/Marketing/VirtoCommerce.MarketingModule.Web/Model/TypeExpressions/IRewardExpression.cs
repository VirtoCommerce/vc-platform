using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions
{
	public interface IRewardExpression
	{
		coreModel.PromotionReward[] GetRewards();
	}
}