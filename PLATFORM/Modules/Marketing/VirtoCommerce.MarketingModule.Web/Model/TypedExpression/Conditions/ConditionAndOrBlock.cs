using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions
{
	public abstract class ConditionAndOrBlock : CompositeElement, IConditionExpression
	{
		public bool All { get; set; }
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var retVal = All ? PredicateBuilder.True<IEvaluationContext>() : PredicateBuilder.False<IEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()))
			{
				retVal = !All ? retVal.Or(expression) : retVal.And(expression);
			}
			return retVal;
		}

		#endregion

		
	}
}