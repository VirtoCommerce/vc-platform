using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    /// <summary>
    /// AndOrBlock for Expression Builder. Can't move to VirtoCommerce.ManagementClient.Core as it references VirtoCommerce.Foundation
    /// </summary>
    [Serializable]
	public class ConditionAndOrBlock : PromotionExpressionBlock
    {
		public ConditionAndOrBlock(string namePrefix, IExpressionViewModel expressionViewModel, string nameSuffix)
			: base(namePrefix + " ... " + nameSuffix, expressionViewModel)
        {
            if (!string.IsNullOrEmpty(namePrefix)) WithLabel(namePrefix);
            AllAny = WithElement(new AllAny()) as AllAny;
            if (!string.IsNullOrEmpty(nameSuffix)) WithLabel(nameSuffix);
        }

        public AllAny AllAny { get; set; }

        public override System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            var retVal = AllAny.IsAll ? PredicateBuilder.True<IEvaluationContext>() : PredicateBuilder.False<IEvaluationContext>();
            foreach (var expression in Children.OfType<IExpressionMarketingAdaptor>().Select(adaptor => adaptor.GetExpression()))
            {
	            retVal = !AllAny.IsAll ? retVal.Or(expression) : retVal.And(expression);
            }
            return retVal;
        }

        public override PromotionReward[] GetPromotionRewards()
        {
            var retVal = new List<PromotionReward>();
            foreach (var adaptor in Children.OfType<IExpressionMarketingAdaptor>())
            {
                retVal.AddRange(adaptor.GetPromotionRewards());
            }
            return retVal.ToArray();
        }
    }
}
