using System;
using System.Linq.Expressions;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IExpressionAdaptor
	{
		Expression<Func<IEvaluationContext, bool>> GetExpression();
	}
}
