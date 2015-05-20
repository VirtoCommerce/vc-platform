using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.DynamicExpressionModule.Data
{
	public interface IConditionExpression
	{
		System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression();
	
	}
}