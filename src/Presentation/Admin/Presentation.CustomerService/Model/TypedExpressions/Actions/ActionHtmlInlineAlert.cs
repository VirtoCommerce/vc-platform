using System;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	[Serializable]
	public class ActionHtmlInlineAlert : TypedExpressionElementBase, IExpressionCaseAlertsAdaptor
	{
		private MultiLineTextBoxInputElement _htmlTemplateEl;
		public ActionHtmlInlineAlert(IExpressionViewModel expressionViewModel)
			: base("Raise Html based alert".Localize(), expressionViewModel)
		{
			WithLabel("Html based alert: ".Localize());
			_htmlTemplateEl = WithElement(new MultiLineTextBoxInputElement(expressionViewModel, "Html".Localize(), "Enter Html".Localize(), defaultHtml)) as MultiLineTextBoxInputElement;
		}

		public CaseAlert[] GetCaseAlerts()
		{
			var retVal = new CaseAlert
			{
				HtmlBody = ValueString
			};
			return new CaseAlert[] { retVal };
		}

		public string ValueString
		{
			get
			{
				return Convert.ToString(_htmlTemplateEl.InputValue);
			}
			set
			{
				_htmlTemplateEl.InputValue = value;
			}
		}

		public virtual System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			throw new NotImplementedException();
		}

		private const string defaultHtml = @"
<html>
	<body>
		<h2>Case</h2>
	</body>
</html>";
	}
}
