using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DynamicExpressionModule.Data.Content
{
	public class DynamicContentExpressionTree : DynamicExpression, IConditionExpression
	{
		#region IConditionExpression Members

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var retVal = PredicateBuilder.True<IEvaluationContext>();
			foreach (var expression in Children.OfType<IConditionExpression>().Select(x => x.GetConditionExpression()).Where(x=> x != null))
			{
				retVal = retVal.And(expression);
			}
			return retVal;
		}

		#endregion
	}
}