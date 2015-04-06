using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using dataModel = VirtoCommerce.MarketingModule.Data;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//[] [] items of category are in shopping cart
	public class ConditionAtNumItemsInCategoryAreInCart : ConditionBase, IConditionExpression
	{
	
		public decimal NumItem { get; set; }
	
		public bool Exactly { get; set; }

		public string SelectedCategoryId { get; set; }
	

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetItemsOfCategoryQuantity(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds) > NumItem
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(dataModel.PromotionEvaluationContext));
			var methodInfo = typeof(dataModel.PromotionEvaluationContextExtension).GetMethod("GetItemsOfCategoryQuantity");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(SelectedCategoryId), GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}
