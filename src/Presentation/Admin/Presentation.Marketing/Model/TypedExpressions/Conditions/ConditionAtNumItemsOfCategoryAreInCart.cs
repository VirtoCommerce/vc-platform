using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionAtNumItemsInCategoryAreInCart : PromotionExpressionBlock
	{
		private readonly UserInputElement _numItemEl;
		private readonly ItemsInCategory _itemsInCategoryEl;

		public ConditionAtNumItemsInCategoryAreInCart(IExpressionViewModel expressionViewModel)
			: base("[] [] items of category are in shopping cart".Localize(), expressionViewModel)
		{
			ExactlyLeast = WithElement(new ExactlyLeast()) as ExactlyLeast;
			_numItemEl = WithUserInput(1, 0) as UserInputElement;
			_itemsInCategoryEl = WithElement(new ItemsInCategory(expressionViewModel)) as ItemsInCategory;
			WithLabel("are in shopping cart".Localize());
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInSku(expressionViewModel));
		}

		public decimal NumItem
		{
			get
			{
				return Convert.ToDecimal(_numItemEl.InputValue);
			}
			set
			{
				_numItemEl.InputValue = value;
			}
		}

		public ExactlyLeast ExactlyLeast { get; set; }

		public string SelectedCategoryId
		{
			get
			{
				return _itemsInCategoryEl.SelectedCategoryId;
			}
			set
			{
				_itemsInCategoryEl.SelectedCategoryId = value;
			}
		}


		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetItemsOfCategoryQuantity");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedCategoryId), ExcludingCategoryIds.GetNewArrayExpression(),
																	  ExcludingProductIds.GetNewArrayExpression(), ExcludingSkuIds.GetNewArrayExpression());
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = ExactlyLeast.IsExactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

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
