using System;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
	[Serializable]
	public class ConditionItemStoreIs : TypedExpressionElementBase, IExpressionAdaptor
	{
		private StoreElement _itemsInStoreEl;

		public ConditionItemStoreIs(IExpressionViewModel expressionViewModel)
			: base("Item is in store []".Localize(), expressionViewModel)
		{
			WithLabel("Item is in store ".Localize());
			_itemsInStoreEl = WithElement(new StoreElement(expressionViewModel)) as StoreElement;
		}

		public string SelectedStoreId
		{
			get
			{
				return Convert.ToString(_itemsInStoreEl.InputValue);
			}
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DisplayTemplateEvaluationContext));
			var methodInfo = typeof(DisplayTemplateEvaluationContext).GetMethod("ItemIsInStore");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedStoreId));
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			_itemsInStoreEl.InitializeAvailableValues(expressionViewModel);
			//WithAvailableExcluding(() => new CustomerInStore(expressionViewModel));
		}
	}
}
