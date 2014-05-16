using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionLineItemTotal : PromotionExpressionBlock
	{
		private readonly UserInputElement _lineItemTotalEl;

		public ConditionLineItemTotal(IExpressionViewModel expressionViewModel)
			: base("Line item subtotal is []".Localize(), expressionViewModel)
		{
			WithLabel("Line item subtotal is ".Localize());
			ExactlyLeast = WithElement(new ExactlyLeast()) as ExactlyLeast;
			_lineItemTotalEl = WithUserInput<decimal>(0, 0) as UserInputElement;
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}

		public decimal LineItemTotal
		{
			get
			{
				return Convert.ToDecimal(_lineItemTotalEl.InputValue);
			}
			set
			{
				_lineItemTotalEl.InputValue = value;
			}
		}

		public ExactlyLeast ExactlyLeast { get; set; }

		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var lineItemTotal = linq.Expression.Constant(LineItemTotal);
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsAnyLineItemTotal");
			var equalsOrAtLeast = ExactlyLeast.IsExactly ? linq.Expression.Constant(true) : linq.Expression.Constant(false);
			var methodCall = linq.Expression.Call(castOp, methodInfo, lineItemTotal, equalsOrAtLeast, ExcludingCategoryIds.GetNewArrayExpression(),
																	  ExcludingProductIds.GetNewArrayExpression(), ExcludingSkuIds.GetNewArrayExpression());

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}
	}
}