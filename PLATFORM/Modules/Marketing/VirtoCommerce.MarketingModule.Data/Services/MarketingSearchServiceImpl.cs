using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using Omu.ValueInjecter;
using VirtoCommerce.CustomerModule.Data.Converters;
using ExpressionSerialization;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class MarketingSearchServiceImpl : IMarketingSearchService
	{
		private readonly Func<IMarketingRepository> _repositoryFactory;
		private readonly IMarketingExtensionManager _customPromotionManager;

		public MarketingSearchServiceImpl(Func<IMarketingRepository> repositoryFactory, IMarketingExtensionManager customPromotionManager)
		{
			_repositoryFactory = repositoryFactory;
			_customPromotionManager = customPromotionManager;
		}


		#region IMarketingSearchService Members

		public coreModel.MarketingSearchResult SearchResources(coreModel.MarketingSearchCriteria criteria)
		{
			var retVal = new coreModel.MarketingSearchResult();
			var count = criteria.Count;
			
			if ((criteria.ResponseGroup & coreModel.SearchResponseGroup.WithPromotions) == coreModel.SearchResponseGroup.WithPromotions)
			{
				SearchPromotions(criteria, retVal);
				criteria.Count -= retVal.Promotions.Count();
			}
			if ((criteria.ResponseGroup & coreModel.SearchResponseGroup.WithContentItems) == coreModel.SearchResponseGroup.WithContentItems)
			{
				SearchContentItems(criteria, retVal);
				criteria.Count -= retVal.ContentItems.Count();
			}
			if ((criteria.ResponseGroup & coreModel.SearchResponseGroup.WithContentPlaces) == coreModel.SearchResponseGroup.WithContentPlaces)
			{
				SearchContentPlaces(criteria, retVal);
				criteria.Count -= retVal.ContentPlaces.Count();
			}
			if ((criteria.ResponseGroup & coreModel.SearchResponseGroup.WithContentPublications) == coreModel.SearchResponseGroup.WithContentPublications)
			{
				SearchContentPublications(criteria, retVal);
				criteria.Count -= retVal.ContentPublications.Count();
			}
			if ((criteria.ResponseGroup & coreModel.SearchResponseGroup.WithFolders) == coreModel.SearchResponseGroup.WithFolders)
			{
				SearchFolders(criteria, retVal);
			}
			
			return retVal;
		}

		#endregion
		private void SearchFolders(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
		{
			using (var repository = _repositoryFactory())
			{
				var query = repository.Folders.Where(x => x.ParentFolderId == criteria.FolderId);
				var folderIds = query.Select(x => x.Id).ToArray();

				result.ContentFolders = new List<coreModel.DynamicContentFolder>();
				foreach(var folderId in folderIds)
				{
					var folder = repository.GetContentFolderById(folderId);
					result.ContentFolders.Add(folder.ToCoreModel());
				}

				//Populate folder for all founded places and items
				if (criteria.FolderId != null)
				{
					var searchedFolder = repository.GetContentFolderById(criteria.FolderId);
					if(searchedFolder != null)
					{
						var hasfolderItems = result.ContentPlaces.OfType<coreModel.IsHasFolder>().Concat(result.ContentItems);
						foreach(var hasfolderItem in hasfolderItems)
						{
							hasfolderItem.Folder = searchedFolder.ToCoreModel();
						}
					}
				}

			}
		}

		private void SearchContentItems(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
		{
			using (var repository = _repositoryFactory())
			{
				var query = repository.Items.Include(x => x.PropertyValues).Where(x => x.FolderId == criteria.FolderId);
				result.TotalCount += query.Count();

				result.ContentItems = query.OrderBy(x => x.Id)
											  .Skip(criteria.Start)
											  .Take(criteria.Count)
											  .ToArray()
											  .Select(x => x.ToCoreModel())
											  .ToList();
			}

		}

		private void SearchContentPlaces(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
		{
			using (var repository = _repositoryFactory())
			{
				var query = repository.Places.Where(x => x.FolderId == criteria.FolderId);
				result.TotalCount += query.Count();

				result.ContentPlaces = query.OrderBy(x => x.Id)
											  .Skip(criteria.Start)
											  .Take(criteria.Count)
											  .ToArray()
											  .Select(x => x.ToCoreModel())
											  .ToList();
			}

		}

		private void SearchContentPublications(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
		{
			using (var repository = _repositoryFactory())
			{
				var query = repository.PublishingGroups;
				result.TotalCount += query.Count();

				result.ContentPublications = query.OrderBy(x => x.Id)
											  .Skip(criteria.Start)
											  .Take(criteria.Count)
											  .ToArray()
											  .Select(x => x.ToCoreModel())
											  .ToList();
			}

		}
		private void SearchPromotions(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
		{
			var promotions = new List<coreModel.Promotion>();
			var totalCount = 0;
			using (var repository = _repositoryFactory())
			{
				promotions = repository.Promotions.OrderBy(x => x.Id)
											  .Skip(criteria.Start)
											  .Take(criteria.Count)
											  .ToArray()
											  .Select(x => x.ToCoreModel())
											  .ToList();
				totalCount = repository.Promotions.Count();
			}


			promotions.AddRange(_customPromotionManager.Promotions.Skip(criteria.Start).Take(criteria.Count));
			totalCount += _customPromotionManager.Promotions.Count();

			result.Promotions = promotions;
			result.TotalCount += totalCount;
		}
	}
}
