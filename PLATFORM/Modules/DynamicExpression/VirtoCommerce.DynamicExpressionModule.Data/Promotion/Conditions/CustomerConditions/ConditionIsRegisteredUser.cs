using System;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
	//Registered user
	public class ConditionIsRegisteredUser : ConditionBase, IConditionExpression
	{
		#region IConditionExpression Members
		/// <summary>
		///  ((PromotionEvaluationContext)x).IsRegisteredUser
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var memberInfo = typeof(PromotionEvaluationContext).GetMember("IsRegisteredUser").First();
			var isRegistered = linq.Expression.MakeMemberAccess(castOp, memberInfo);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(isRegistered, paramX);

			return retVal;
		}

		#endregion
	}
}
