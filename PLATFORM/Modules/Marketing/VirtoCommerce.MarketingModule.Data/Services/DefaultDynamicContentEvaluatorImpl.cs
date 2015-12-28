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
using VirtoCommerce.Domain.Common;
using VirtoCommerce.CoreModule.Data.Common;
using Common.Logging;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class DefaultDynamicContentEvaluatorImpl : IMarketingDynamicContentEvaluator
	{
		private readonly Func<IMarketingRepository> _repositoryFactory;
		private readonly IDynamicContentService _dynamicContentService;
        private readonly ILog _logger;
        public DefaultDynamicContentEvaluatorImpl(Func<IMarketingRepository> repositoryFactory, IDynamicContentService dynamicContentService, ILog logger)
		{
			_repositoryFactory = repositoryFactory;
			_dynamicContentService = dynamicContentService;
            _logger = logger;
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
            using (var repository = _repositoryFactory())
			{
                var publishings = repository.PublishingGroups.Include(x => x.ContentItems)
                                                       .Where(x => x.IsActive)
                                                       .Where(x => x.StoreId == dynamicContext.StoreId)
                                                       .Where(x => (x.StartDate == null || dynamicContext.ToDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= dynamicContext.ToDate))
                                                       .Where(x => x.ContentPlaces.Any(y => y.ContentPlace.Name == dynamicContext.PlaceName))
                                                       .OrderBy(x => x.Priority)
                                                       .ToArray();

                //Get content items ids for publishings without ConditionExpression
                var contentItemIds = publishings.Where(x => x.ConditionExpression == null)
                                                .SelectMany(x => x.ContentItems)
                                                .Select(x => x.DynamicContentItemId)
                                                .ToList();
                foreach (var publishing in publishings.Where(x=>x.ConditionExpression != null))
                {
                    try
                    {
                        //Next step need filter assignments contains dynamicexpression
                        var condition = SerializationUtil.DeserializeExpression<Func<IEvaluationContext, bool>>(publishing.ConditionExpression);
                        if (condition(context))
                        {
                            contentItemIds.AddRange(publishing.ContentItems.Select(x=>x.DynamicContentItemId));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }

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
