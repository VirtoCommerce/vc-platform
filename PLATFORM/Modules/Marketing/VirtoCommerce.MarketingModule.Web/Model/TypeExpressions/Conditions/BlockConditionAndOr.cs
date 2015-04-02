using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	public abstract class BlockConditionAndOr : DynamicExpressionBase, IConditionExpression
	{
		public bool All { get; set; }
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			System.Linq.Expressions.Expression<Func<IPromotionEvaluationContext, bool>> retVal = null;
			if (Children != null && Children.Any())
			{
				retVal = All ? PredicateBuilder.True<IPromotionEvaluationContext>() : PredicateBuilder.False<IPromotionEvaluationContext>();
				foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()))
				{
					retVal = All ? retVal.And(expression) : retVal.Or(expression);
				}
			}
			return retVal;
		}

		#endregion

		
	}
}