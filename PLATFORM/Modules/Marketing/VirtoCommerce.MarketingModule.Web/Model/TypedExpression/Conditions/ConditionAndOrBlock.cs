using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions
{
	public class ConditionAndOrBlock : CompositeElement, IConditionExpression, IRewardExpression
	{
		public bool All { get; set; }
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var retVal = All ? PredicateBuilder.True<IEvaluationContext>() : PredicateBuilder.False<IEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetExpression()))
			{
				retVal = !All ? retVal.Or(expression) : retVal.And(expression);
			}
			return retVal;
		}

		#endregion

		#region IRewardExpression Members

		//public PromotionReward[] GetPromotionRewards()
		//{
		   //var retVal = new List<PromotionReward>();
		   // foreach (var adaptor in Children.OfType<IExpressionMarketingAdaptor>())
		   // {
		   //	 retVal.AddRange(adaptor.GetPromotionRewards());
		   // }
		   // return retVal.ToArray();
		//}

		#endregion
	}
}