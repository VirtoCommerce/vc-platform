using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions
{
	public class ConditionAtNumItemsInCart : CompositeElement, IConditionExpression
	{
		public decimal NumItem
		{
			get;
			set;
		}

		public bool Exactly { get; set; }


		#region IConditionExpression Members

		System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> IConditionExpression.GetExpression()
		{
			//var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			//var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			//var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetItemsQuantity");
			//var methodCall = linq.Expression.Call(castOp, methodInfo, ExcludingCategoryIds.GetNewArrayExpression(),
			//														  ExcludingProductIds.GetNewArrayExpression(), ExcludingSkuIds.GetNewArrayExpression());
			//var numItem = linq.Expression.Constant(NumItem);
			//var binaryOp = ExactlyLeast.IsExactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			//var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);
			//return retVal;
			return null;
		}

		#endregion
	}
}