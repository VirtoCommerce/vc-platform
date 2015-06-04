using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DynamicExpressionModule.Data
{
	public abstract class BlockConditionAndOr : DynamicExpression, IConditionExpression
	{
		public bool All { get; set; }
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> retVal = null;
			if (Children != null && Children.Any())
			{
				retVal = All ? PredicateBuilder.True<IEvaluationContext>() : PredicateBuilder.False<IEvaluationContext>();
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