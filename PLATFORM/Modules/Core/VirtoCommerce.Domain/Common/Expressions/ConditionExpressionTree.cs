using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Common
{
	public class ConditionExpressionTree : DynamicExpression, IConditionExpression
	{
		#region IConditionExpression Members

		public virtual System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var retVal = PredicateBuilder.True<IEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()).Where(x => x != null))
			{
				retVal = retVal.And(expression);
			}
			return retVal;
		}

		#endregion
	}
}
