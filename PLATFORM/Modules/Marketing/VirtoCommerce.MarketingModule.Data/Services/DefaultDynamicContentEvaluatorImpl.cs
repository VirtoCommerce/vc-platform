using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Domain.Common;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class DefaultDynamicContentEvaluatorImpl : IMarketingDynamicContentEvaluator
	{
		private readonly Func<IMarketingRepository> _repositoryFactory;
		private readonly IDynamicContentService _dynamicContentService;
		public DefaultDynamicContentEvaluatorImpl(Func<IMarketingRepository> repositoryFactory, IDynamicContentService dynamicContentService)
		{
			_repositoryFactory = repositoryFactory;
			_dynamicContentService = dynamicContentService;
		}
		#region IMarketingDynamicContentEvaluator Members

		public DynamicContentItem[] EvaluateItems(IEvaluationContext context)
		{
			var dynamicContext = context as DynamicContentEvaluationContext;
			if(context == null)
			{
				throw new ArgumentNullException("dynamicContext");
			}
			var retVal = new List<DynamicContentItem>();
			using(var repository = _repositoryFactory())
			{
				var query = repository.PublishingGroups.Include(x => x.ContentItems)
													   .Where(x => x.IsActive)
													   .Where(x => (x.StartDate == null || dynamicContext.ToDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= dynamicContext.ToDate))
													   .Where(x => x.ContentPlaces.Any(y => y.ContentPlace.Name == dynamicContext.PlaceName))
													   .OrderBy(x => x.Priority)
													   .SelectMany(x => x.ContentItems)
													   .Select(x => x.DynamicContentItemId);
				var contentItemIds = query.ToArray();
				foreach (var contentItemId in contentItemIds)
				{
					var contentItem = _dynamicContentService.GetContentItemById(contentItemId);
					retVal.Add(contentItem);
				}
			
			}

			return retVal.ToArray();
		}

		#endregion
	}
}
