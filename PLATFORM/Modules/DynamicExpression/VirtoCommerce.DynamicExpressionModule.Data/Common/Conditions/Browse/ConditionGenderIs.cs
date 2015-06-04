using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
    //Shopper gender is []
    public class ConditionGenderIs : MatchedConditionBase<EvaluationContextBase>
    {
        public ConditionGenderIs()
            : base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x => x.ShopperGender))
        {
            Value = "female";
        }
    }
}