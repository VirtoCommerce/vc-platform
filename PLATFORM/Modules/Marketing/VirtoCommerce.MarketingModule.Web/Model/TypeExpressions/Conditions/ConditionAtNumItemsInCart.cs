using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using dataModel = VirtoCommerce.MarketingModule.Data;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//[] [] items are in shopping cart
	public class ConditionAtNumItemsInCart : ConditionBase, IConditionExpression
	{
		public decimal NumItem { get; set; }

		public bool Exactly { get; set; }

		
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetItemsQuantity(ExcludingCategoryIds, ExcludingProductIds) > NumItem
		/// </summary>
		/// <returns></returns>
		linq.Expression<Func<IPromotionEvaluationContext, bool>> IConditionExpression.GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(dataModel.PromotionEvaluationContext));
			var methodInfo = typeof(dataModel.PromotionEvaluationContextExtension).GetMethod("GetItemsQuantity");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, GetNewArrayExpression(ExcludingCategoryIds),
																	 GetNewArrayExpression(ExcludingProductIds));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);
			return retVal;
		}

		#endregion
	}
}