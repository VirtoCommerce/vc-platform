using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions
{
	public class PromoDynamicExpression : DynamicBlockExpression, IConditionExpression, IRewardExpression
	{
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var retVal = PredicateBuilder.True<IPromotionEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()).Where(x=> x != null))
			{
				retVal = retVal.And(expression);
			}
			return retVal;
		}

		#endregion

		#region IActionExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = Children.OfType<IRewardExpression>().SelectMany(x => x.GetRewards()).OfType<coreModel.PromotionReward>().ToArray();

			return retVal;
		}

		#endregion
	}
}