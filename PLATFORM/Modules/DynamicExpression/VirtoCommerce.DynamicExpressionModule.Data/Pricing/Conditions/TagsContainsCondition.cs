using System;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Pricing.Model;
using linq = System.Linq.Expressions;
using VirtoCommerce.DynamicExpressionModule.Data.Pricing;
namespace VirtoCommerce.DynamicExpressionModule.Data.Pricing
{
    //Tags contains []
    public class TagsContainsCondition : DynamicExpression, IConditionExpression
    {
        public string Tag { get; set; }

        #region IConditionExpression Members
        /// <summary>
        /// ((PriceEvaluationContext)x).TagsContainsKeyword(Keyword)
        /// </summary>
        /// <returns></returns>
        public linq.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
        {
            var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
            var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PriceEvaluationContext));
            var methodInfo = typeof(PricingEvaluationContextExtension).GetMethod("TagsContains");

            var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(Tag));

            var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

            return retVal;
        }

        #endregion
    }
}
