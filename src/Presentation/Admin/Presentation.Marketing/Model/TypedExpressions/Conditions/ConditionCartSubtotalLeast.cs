using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionCartSubtotalLeast : PromotionExpressionBlock
	{
		private readonly UserInputElement _subTotalEl;

		public ConditionCartSubtotalLeast(IExpressionViewModel expressionViewModel)
			: base("Cart subtotal is []".Localize(), expressionViewModel)
		{
			WithLabel("Cart subtotal is ".Localize());
			ExactlyLeast = WithElement(new ExactlyLeast()) as ExactlyLeast;
			_subTotalEl = WithUserInput<decimal>(0, 0) as UserInputElement;
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}

		public decimal SubTotal
		{
			get
			{
				return Convert.ToDecimal(_subTotalEl.InputValue);
			}
			set
			{
				_subTotalEl.InputValue = value;
			}
		}

		public ExactlyLeast ExactlyLeast { get; set; }

		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var subTotal = linq.Expression.Constant(SubTotal);
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetTotalWithExcludings");
			var methodCall = linq.Expression.Call(castOp, methodInfo, ExcludingCategoryIds.GetNewArrayExpression(),
																	  ExcludingProductIds.GetNewArrayExpression(), ExcludingSkuIds.GetNewArrayExpression());

			var binaryOp = ExactlyLeast.IsExactly ? linq.Expression.Equal(methodCall, subTotal) : linq.Expression.GreaterThanOrEqual(methodCall, subTotal);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

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