using System;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	public interface IDisplayTemplateEvaluationContext : IEvaluationContext
	{
		TargetTypes TargetType { get; set; }
	}
}
