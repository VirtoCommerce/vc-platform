using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionIsFirstTimeBuyer : PromotionExpressionBlock
	{
		public ConditionIsFirstTimeBuyer(IExpressionViewModel expressionViewModel)
			: base("First time buyers".Localize(), expressionViewModel)
		{
			WithLabel("First time buyers".Localize());
		}

		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var memberInfo = typeof(PromotionEvaluationContext).GetMember("IsFirstTimeBuyer").First();
			var isRegistered = linq.Expression.MakeMemberAccess(castOp, memberInfo);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(isRegistered, paramX);

			return retVal;
		}
	}
}
