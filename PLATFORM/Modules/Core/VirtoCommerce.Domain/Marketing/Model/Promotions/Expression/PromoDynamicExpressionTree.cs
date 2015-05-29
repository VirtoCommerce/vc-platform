using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class PromoDynamicExpressionTree : ConditionExpressionTree, IRewardExpression
	{
		#region IActionExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = Children.OfType<IRewardExpression>().SelectMany(x => x.GetRewards()).OfType<PromotionReward>().ToArray();

			return retVal;
		}

		#endregion
	}
}