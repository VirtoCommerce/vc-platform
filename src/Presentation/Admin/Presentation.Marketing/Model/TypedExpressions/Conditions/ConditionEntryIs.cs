using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using linq = System.Linq.Expressions;


namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public class ConditionEntryIs : PromotionExpressionBlock
    {
        private readonly ItemsInEntry _itemsInEntryEl;

		public ConditionEntryIs(IExpressionViewModel expressionViewModel)
			: base("Entry is []", expressionViewModel)
        {
            WithLabel("Entry is ");
			_itemsInEntryEl = WithElement(new ItemsInEntry(expressionViewModel)) as ItemsInEntry;
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


        public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            //Func<IPromotionEvaluationContext, bool> predicate = (x) => ((PromotionEvaluationContext)x).IsItemInCategory(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds);

            var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
            var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
            var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemInProduct");
            var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedItemId));
            var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

            return retVal;
        }
    }
}