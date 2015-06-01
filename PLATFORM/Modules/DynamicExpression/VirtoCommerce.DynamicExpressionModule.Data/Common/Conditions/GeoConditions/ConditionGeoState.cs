using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
    //State is []
    public class ConditionGeoState : CompareConditionBase<EvaluationContextBase>
    {
		public ConditionGeoState()
            : base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.GeoState))
        {
        }

    }
}