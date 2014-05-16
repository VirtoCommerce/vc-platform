using System;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	[Serializable]
	public class ActionRedirectAlert : TypedExpressionElementBase, IExpressionCaseAlertsAdaptor
	{
		private UserInputElement _redirectEl;
		public ActionRedirectAlert(IExpressionViewModel expressionViewModel)
			: base("Raise redirect alert".Localize(), expressionViewModel)
		{
			WithLabel("Raise redirect alert\r\n  (use {CaseId}, {ContactId} as parameters)".Localize());
			_redirectEl = WithUserInput<string>(string.Empty) as UserInputElement;
			_redirectEl.DefaultValue = @"...?customerid={ContactId}&caseid={CaseId}";
		}

		public CaseAlert[] GetCaseAlerts()
		{
			var retVal = new CaseAlert
			{
				RedirectUrl = ValueString
			};
			return new CaseAlert[] { retVal };
		}

		public string ValueString
		{
			get
			{
				return Convert.ToString(_redirectEl.InputValue);
			}
			set
			{
				_redirectEl.InputValue = value;
			}
		}

		public virtual System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			throw new NotImplementedException();
		}
	}
}
