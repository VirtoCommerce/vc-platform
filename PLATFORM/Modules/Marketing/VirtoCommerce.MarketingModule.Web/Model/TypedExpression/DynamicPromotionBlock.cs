using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression
{
	public class DynamicPromotionBlock : CompositeElement, IConditionExpression, IRewardExpression
	{
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var retVal =  PredicateBuilder.True<IEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()))
			{
				retVal = retVal.And(expression);
			}
			return retVal;
		}

		#endregion

		#region IActionExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = Children.OfType<IRewardExpression>().SelectMany(x => x.GetRewards()).ToArray();

			return retVal;
		}

		#endregion
	}
}