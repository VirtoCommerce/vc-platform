using System;
using System.Linq;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Services
{
	public class DynamicContentService : IDynamicContentService
	{
		private IDynamicContentRepository _repository;
		private readonly IDynamicContentEvaluator _evaluator;

		public DynamicContentService(IDynamicContentRepository repository, IDynamicContentEvaluator evaluator)
		{
			_repository = repository;
			_evaluator = evaluator;
		}

        public DynamicContentItem[] GetItems(string placeName, DateTime now, TagSet tags)
		{
            return _evaluator.Evaluate(new DynamicContentEvaluationContext() { CurrentDate = now, ContentPlace = placeName, ContextObject = tags });
		}
	}
}
