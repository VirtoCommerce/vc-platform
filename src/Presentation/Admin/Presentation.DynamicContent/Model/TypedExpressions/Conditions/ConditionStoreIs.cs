using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public class ConditionStoreIs : TypedExpressionElementBase, IExpressionAdaptor
    {
        private readonly StoreElement _itemsInStoreEl;

        public ConditionStoreIs(IExpressionViewModel expressionViewModel)
            : base("Current store is []".Localize(), expressionViewModel)
        {
            WithLabel("Current store is ".Localize());
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
            var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
            var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
            var methodInfo = typeof(DynamicContentEvaluationContext).GetMethod("IsShopperInStore");
            var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedStoreId));
            var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

            return retVal;
        }

        public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
        {
            base.InitializeAfterDeserialized(expressionViewModel);
			_itemsInStoreEl.InitializeAvailableValues(expressionViewModel);
        }
    }
}
