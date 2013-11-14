using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	public static class TypedExpressionExtension
	{
		public static NewArrayExpression GetNewArrayExpression(this IEnumerable<string> items)
		{
			var trees = new List<System.Linq.Expressions.Expression>();
			trees.AddRange(items.Select(System.Linq.Expressions.Expression.Constant));
			return System.Linq.Expressions.Expression.NewArrayInit(typeof(string), trees);
		}
	}
}
