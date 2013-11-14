using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using linq = System.Linq.Expressions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionCategoryIs : PromotionExpressionBlock
	{
		private readonly ItemsInCategory _itemsInCategoryEl;
		private const string EntryLabel = "Category is []";

		public ConditionCategoryIs(IExpressionViewModel expressionViewModel)
			: base(EntryLabel, expressionViewModel)
		{
			_itemsInCategoryEl = WithElement(new ItemsInCategory(expressionViewModel)) as ItemsInCategory;
			WithAvailableExcluding(() => new ItemsInCategory(expressionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(expressionViewModel));
		}


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
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemInCategory");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedCategoryId), ExcludingCategoryIds.GetNewArrayExpression(),
												  ExcludingProductIds.GetNewArrayExpression());
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel promotionViewModel)
		{
			base.InitializeAfterDeserialized(promotionViewModel);
			WithAvailableExcluding(() => new ItemsInCategory(promotionViewModel));
			WithAvailableExcluding(() => new ItemsInEntry(promotionViewModel));
		}
	}
}
