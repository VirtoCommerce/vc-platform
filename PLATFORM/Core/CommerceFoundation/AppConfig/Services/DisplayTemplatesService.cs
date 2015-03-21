using System;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Services
{
	public class DisplayTemplatesService : IDisplayTemplatesService
	{
		#region Dependencies
		private readonly IDisplayTemplateEvaluator _evaluator;
		#endregion

		#region ctor
		public DisplayTemplatesService(IDisplayTemplateEvaluator evaluator)
		{
			_evaluator = evaluator;
		}
		#endregion

		#region IDisplayTemplatesService members
		public string GetTemplate(TargetTypes targetType, TagSet tags)
		{
			return _evaluator.Evaluate(new DisplayTemplateEvaluationContext() { TargetType = targetType, ContextObject = tags });
		}
		#endregion
	}
}
