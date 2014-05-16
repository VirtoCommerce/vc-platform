using System;
using System.Linq;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	[Serializable]
	public class ConditionCaseStatus : TypedExpressionElementBase, IExpressionAdaptor
	{
		private UserInputElement _caseStatusEl;
		public ConditionCaseStatus(IExpressionViewModel expressionViewModel)
			: base("Case status".Localize(), expressionViewModel)
		{
			WithLabel("Case status is ".Localize());
			_caseStatusEl = WithDict(new string[] { "Open", "Pending", "Closed" }, "Open") as UserInputElement;
		}

		public string SelectedStatus
		{
			get
			{
				return _caseStatusEl.InputValue.ToString();
			}
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(CaseAlertEvaluationContext));
			var memberInfo = typeof(CaseAlertEvaluationContext).GetMember("CaseStatus").First();
			var caseStatus = linq.Expression.MakeMemberAccess(castOp, memberInfo);
			var selectedStatus = linq.Expression.Constant(SelectedStatus);
			var binaryOp = linq.Expression.Equal(selectedStatus, caseStatus);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
	}
}
