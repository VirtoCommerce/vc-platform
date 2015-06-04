using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Platform.Core.Common;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
	//CUrrent url is []
	public class ConditionUrlIs : MatchedConditionBase<EvaluationContextBase>
	{
		public ConditionUrlIs()
			: base(ReflectionUtility.GetPropertyName<EvaluationContextBase>(x=>x.CurrentUrl))
		{
		}

	}
}