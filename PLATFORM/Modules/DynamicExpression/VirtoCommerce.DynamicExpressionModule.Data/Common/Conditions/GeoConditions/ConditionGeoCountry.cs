using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
    //Country is []
    public class ConditionGeoCountry : CompareConditionBase<EvaluationContextBase>
    {
		public ConditionGeoCountry()
            : base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.GeoCountry))
        {
        }

    }
}