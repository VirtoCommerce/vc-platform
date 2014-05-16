using System;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	[Serializable]
	public class ActionXslInlineAlert : TypedExpressionElementBase, IExpressionCaseAlertsAdaptor
	{
		private MultiLineTextBoxInputElement _xslTemplateEl;
		public ActionXslInlineAlert(IExpressionViewModel expressionViewModel)
			: base("Raise Xsl based alert".Localize(), expressionViewModel)
		{
			WithLabel("Raise alert using Xsl transformation: ".Localize());
			_xslTemplateEl = WithElement(new MultiLineTextBoxInputElement(expressionViewModel, "Xsl stylesheet".Localize(), "Enter Xsl stylesheet".Localize(), defaultXsl)) as MultiLineTextBoxInputElement;
		}

		public CaseAlert[] GetCaseAlerts()
		{
			var retVal = new CaseAlert
			{
				XslTemplate = ValueString
			};
			return new CaseAlert[] { retVal };
		}

		public string ValueString
		{
			get
			{
				return Convert.ToString(_xslTemplateEl.InputValue);
			}
			set
			{
				_xslTemplateEl.InputValue = value;
			}
		}

		public virtual System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			throw new NotImplementedException();
		}

		private const string defaultXsl = @"
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
	<xsl:template match='/'>
		<html>
			<body>
				<h2>Case</h2>
				<table border='1'>
					<tr bgcolor='#9acd32'>
						<th align='left'>Author</th>
						<th align='left'>Title</th>
						<th align='left'>Status</th>
					</tr>
					<tr>
						<td><xsl:value-of select='Case/Title' /></td>
						<td><xsl:value-of select='Case/Created' /></td>
						<td><xsl:value-of select='Case/Status' /></td>
					</tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>";
	}
}
