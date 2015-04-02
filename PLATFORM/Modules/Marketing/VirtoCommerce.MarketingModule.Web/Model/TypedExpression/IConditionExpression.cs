using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression
{
	public interface IConditionExpression
	{
		System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetConditionExpression();
	
	}
}