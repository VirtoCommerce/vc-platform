using System;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using dataModel = VirtoCommerce.MarketingModule.Data;
namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//First time buyers
	public class ConditionIsFirstTimeBuyer : ConditionBase, IConditionExpression
	{
		#region IConditionExpression Members
		/// <summary>
		///  ((PromotionEvaluationContext)x).IsFirstTimeBuyer
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(dataModel.PromotionEvaluationContext));
			var memberInfo = typeof(dataModel.PromotionEvaluationContext).GetMember("IsFirstTimeBuyer").First();
			var isRegistered = linq.Expression.MakeMemberAccess(castOp, memberInfo);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(isRegistered, paramX);

			return retVal;
		}

		#endregion
	}
}
