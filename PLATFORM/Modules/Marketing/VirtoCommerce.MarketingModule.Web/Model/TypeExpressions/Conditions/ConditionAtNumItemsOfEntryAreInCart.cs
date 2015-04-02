using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.MarketingModule.Data.Services;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//[] [] items of entry are in shopping cart
	public class ConditionAtNumItemsOfEntryAreInCart : ConditionBase, IConditionExpression
	{
		public decimal NumItem { get; set; }
		public bool Exactly { get; set; }
		public string SelectedItemId { get; set; }

		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetItemsOfProductQuantity");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedItemId));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}
