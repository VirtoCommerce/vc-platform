using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions
{
	public abstract class ConditionBase : CompositeElement
	{
		public ConditionBase()
		{
			ExcludingCategoryIds = new List<string>();
			ExcludingProductIds = new List<string>();
		}
		public ICollection<string> ExcludingCategoryIds
		{
			get;
			set;
		}

		public ICollection<string> ExcludingProductIds
		{
			get;
			set;
		}

		protected static NewArrayExpression GetNewArrayExpression(IEnumerable<string> items)
		{
			var trees = new List<System.Linq.Expressions.Expression>();
			trees.AddRange(items.Select(System.Linq.Expressions.Expression.Constant));
			return System.Linq.Expressions.Expression.NewArrayInit(typeof(string), trees);
		}

	}
}