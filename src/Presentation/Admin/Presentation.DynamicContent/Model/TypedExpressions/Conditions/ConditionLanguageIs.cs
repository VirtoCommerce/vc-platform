using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public class ConditionLanguageIs : TypedExpressionElementBase, IExpressionAdaptor
    {
        private readonly LanguageElement _itemsInLanguageEl;

        public ConditionLanguageIs(IExpressionViewModel expressionViewModel)
            : base("Current language is []".Localize(), expressionViewModel)
        {
            WithLabel("Current language is ".Localize());
            _itemsInLanguageEl = WithElement(new LanguageElement(expressionViewModel)) as LanguageElement;
        }

        public string SelectedLanguageId
        {
            get
            {
                return Convert.ToString(_itemsInLanguageEl.InputValue);
            }
        }

        public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
            var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
            var methodInfo = typeof(DynamicContentEvaluationContext).GetMethod("IsShopperInLanguage");
            var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedLanguageId));
            var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

            return retVal;
        }

        public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
        {
            base.InitializeAfterDeserialized(expressionViewModel);
            _itemsInLanguageEl.InitializeAvailableValues(expressionViewModel);
        }
    }
}
