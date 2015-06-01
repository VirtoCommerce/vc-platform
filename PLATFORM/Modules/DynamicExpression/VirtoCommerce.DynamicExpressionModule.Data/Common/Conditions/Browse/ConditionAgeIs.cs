using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
	//Age is []
	public class ConditionAgeIs : CompareConditionBase<EvaluationContextBase>
	{
		public ConditionAgeIs()
			: base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.ShopperAge))
		{
		}

	}
}