using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionCodeContains : PromotionExpressionBlock
	{
		//private readonly MatchContainsStringElement _skuEl;
		private readonly UserInputElement _skuEl;

		public ConditionCodeContains(IExpressionViewModel expressionViewModel)
			: base("Entry code contains []".Localize(), expressionViewModel)
		{
			WithLabel("Entry code contains ".Localize());
			//_skuEl = WithElement(new MatchContainsStringElement(expressionViewModel)) as MatchContainsStringElement;
			_skuEl = WithUserInput<string>(string.Empty) as UserInputElement;
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}

		public string SkuContains
		{
			get
			{
				return _skuEl.InputValue.ToString();
			}
			set
			{
				_skuEl.InputValue = value;
			}
		}

		//public MatchingContains MatchCondition
		//{
		//	get
		//	{
		//		return _skuEl.MatchingContains;
		//	}
		//}

		//public string MatchConditionValue
		//{
		//	get
		//	{
		//		return _skuEl.MatchingContains.InputValue.ToString();
		//	}
		//	set
		//	{
		//		_skuEl.MatchingContains.InputValue = value;
		//	}
		//}


		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemCodeContains");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SkuContains), ExcludingSkuIds.GetNewArrayExpression());

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}
	}
}
