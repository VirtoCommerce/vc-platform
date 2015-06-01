using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
    //Browsing from a time zone -/+ offset from UTC 
    public class ConditionGeoTimeZone : CompareConditionBase<EvaluationContextBase>
    {
        public ConditionGeoTimeZone()
            : base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.GeoTimeZone))
        {

        }

    }
}