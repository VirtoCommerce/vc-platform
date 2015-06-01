using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
    //Browsing from zip/postal code []
	public class ConditionGeoZipCode : MatchedConditionBase<EvaluationContextBase>
    {
        public ConditionGeoZipCode()
            : base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.GeoZipCode))
        {
        }
    }
}