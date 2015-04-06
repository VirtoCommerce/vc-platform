using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using dataModel = VirtoCommerce.MarketingModule.Data;
namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Line item subtotal is []
	public class ConditionLineItemTotal : ConditionBase, IConditionExpression
	{
		public decimal LineItemTotal { get; set; }

		public bool Exactly { get; set; }
		
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsAnyLineItemTotal(LineItemTotal, Exactly,  ExcludingCategoryIds, ExcludingProductIds)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(dataModel.PromotionEvaluationContext));
			var lineItemTotal = linq.Expression.Constant(LineItemTotal);
			var methodInfo = typeof(dataModel.PromotionEvaluationContextExtension).GetMethod("IsAnyLineItemTotal");

			var equalsOrAtLeast = Exactly ? linq.Expression.Constant(true) : linq.Expression.Constant(false);
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, lineItemTotal, equalsOrAtLeast, GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}