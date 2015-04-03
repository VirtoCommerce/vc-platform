using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Newtonsoft.Json;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	public abstract class ConditionBase : PromoDynamicExpression
	{
		public ConditionBase()
		{
			ExcludingCategoryIds = new List<string>();
			ExcludingProductIds = new List<string>();
		}
		public ICollection<string> ExcludingCategoryIds	{ get; set;	}

		public ICollection<string> ExcludingProductIds { get; set; }

		protected static NewArrayExpression GetNewArrayExpression(IEnumerable<string> items)
		{
			var trees = new List<System.Linq.Expressions.Expression>();
			trees.AddRange(items.Select(System.Linq.Expressions.Expression.Constant));
			return System.Linq.Expressions.Expression.NewArrayInit(typeof(string), trees);
		}

	}
}