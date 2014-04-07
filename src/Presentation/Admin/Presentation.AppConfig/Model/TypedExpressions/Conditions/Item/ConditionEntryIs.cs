using System;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
	[Serializable]
	public class ConditionEntryIs : TypedExpressionElementBase, IExpressionAdaptor
	{
		private ItemSelectElement _itemsInEntryEl;

		public ConditionEntryIs(IExpressionViewModel expressionViewModel)
			: base("Item is []".Localize(), expressionViewModel)
		{
			WithLabel("Item is ".Localize());
			_itemsInEntryEl = WithElement(new ItemSelectElement(expressionViewModel)) as ItemSelectElement;
		}

		public string SelectedItemId
		{
			get
			{
				return _itemsInEntryEl.SelectedItemId;
			}
			set
			{
				_itemsInEntryEl.SelectedItemId = value;
			}
		}


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			//Func<IPromotionEvaluationContext, bool> predicate = (x) => ((PromotionEvaluationContext)x).IsItemInCategory(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds);

			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DisplayTemplateEvaluationContext));
			var methodInfo = typeof(DisplayTemplateEvaluationContext).GetMethod("IsCurrentItem");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedItemId));
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}
	}
}