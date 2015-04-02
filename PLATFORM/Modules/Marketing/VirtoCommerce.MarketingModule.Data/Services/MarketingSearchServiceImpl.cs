using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.MarketingModule.Data.Repositories;
using foundationModel = VirtoCommerce.Foundation.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.CustomerModule.Data.Converters;
using ExpressionSerialization;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class MarketingSearchServiceImpl : IMarketingSearchService
	{
		private readonly Func<IFoundationMarketingRepository> _repositoryFactory;
		private readonly IPromotionExtensionManager _customPromotionManager;

		public MarketingSearchServiceImpl(Func<IFoundationMarketingRepository> repositoryFactory, IPromotionExtensionManager customPromotionManager)
		{
			_repositoryFactory = repositoryFactory;
			_customPromotionManager = customPromotionManager;
		}


		#region IMarketingSearchService Members

		public SearchResult SearchPromotions(SearchCriteria criteria)
		{
			var promotions = new List<coreModel.Promotion>();
			var totalCount = 0;
			using (var repository = _repositoryFactory())
			{
				promotions = repository.Promotions.OrderBy(x => x.PromotionId)
											  .Skip(criteria.Start)
											  .Take(criteria.Count)
											  .ToArray()
											  .Select(x => x.ToCoreModel())
											  .ToList();
				totalCount = repository.Promotions.Count();
			}


			promotions.AddRange(_customPromotionManager.Promotions.Skip(criteria.Start).Take(criteria.Count));
			totalCount += _customPromotionManager.Promotions.Count();


			var retVal = new SearchResult
			{
				Promotions = promotions.OrderBy(x => x.Id).Take(criteria.Count).ToList(),
				TotalCount = totalCount
			};
			return retVal;
		}

		#endregion
	}
}
