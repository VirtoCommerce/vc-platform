using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.MarketingModule.Web.ExportImport
{

    public sealed class BackupObject
    {
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Coupon> Coupons { get; set; }
        public ICollection<DynamicContentPlace> ContentPlaces { get; set; }
        public ICollection<DynamicContentItem> ContentItems { get; set; }
        public ICollection<DynamicContentPublication> ContentPublications { get; set; }
        public ICollection<DynamicContentFolder> ContentFolders { get; set; }
    }

    public sealed class MarketingExportImport
    {
        private readonly IMarketingSearchService _marketingSearchService;
        private readonly IPromotionService _promotionService;
        private readonly IDynamicContentService _dynamicContentService;

        public MarketingExportImport(IMarketingSearchService marketingSearchService, IPromotionService promotionService, IDynamicContentService dynamicContentService)
        {
            _marketingSearchService = marketingSearchService;
            _promotionService = promotionService;
            _dynamicContentService = dynamicContentService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
			var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback);

			var progressInfo = new ExportImportProgressInfo();

			progressInfo.Description = String.Format("{0} promotions importing...", backupObject.Promotions.Count());
			progressCallback(progressInfo);
			UpdatePromotions(originalObject.Promotions, backupObject.Promotions);

			progressInfo.Description = String.Format("{0} folders importing...", backupObject.ContentFolders.Count());
			progressCallback(progressInfo);
            UpdateContentFolders(originalObject.ContentFolders, backupObject.ContentFolders);

			progressInfo.Description = String.Format("{0} places importing...", backupObject.ContentPlaces.Count());
			progressCallback(progressInfo);
            UpdateContentPlaces(originalObject.ContentPlaces, backupObject.ContentPlaces);

			progressInfo.Description = String.Format("{0} contents importing...", backupObject.ContentItems.Count());
			progressCallback(progressInfo);
            UpdateContentItems(originalObject.ContentItems, backupObject.ContentItems);

			progressInfo.Description = String.Format("{0} publications importing...", backupObject.ContentPublications.Count());
			progressCallback(progressInfo);
            UpdateContentPublications(originalObject.ContentPublications, backupObject.ContentPublications);

        }

        #region Import updates
        
        private void UpdatePromotions(ICollection<Promotion> original, ICollection<Promotion> backup)
        {
            var toUpdate = new List<Promotion>();

            backup.CompareTo(original, EqualityComparer<Promotion>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _promotionService.CreatePromotion(x);
                        break;
                }
            });
            _promotionService.UpdatePromotions(toUpdate.ToArray());
        }

        private void UpdateCoupons(ICollection<Coupon> original, ICollection<Coupon> backup)
        {
            var toUpdate = new List<Coupon>();

            backup.CompareTo(original, EqualityComparer<Coupon>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _promotionService.CreateCoupon(x);
                        break;
                }
            });
            _promotionService.UpdateCoupons(toUpdate.ToArray());
        }

        private void UpdateContentPlaces(ICollection<DynamicContentPlace> original, ICollection<DynamicContentPlace> backup)
        {
            backup.CompareTo(original, EqualityComparer<DynamicContentPlace>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        _dynamicContentService.UpdatePlace(x);
                        break;
                    case EntryState.Added:
                        _dynamicContentService.CreatePlace(x);
                        break;
                }
            });
        }

        private void UpdateContentItems(ICollection<DynamicContentItem> original, ICollection<DynamicContentItem> backup)
        {
            var toUpdate = new List<DynamicContentItem>();

            backup.CompareTo(original, EqualityComparer<DynamicContentItem>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _dynamicContentService.CreateContent(x);
                        break;
                }
            });
            _dynamicContentService.UpdateContents(toUpdate.ToArray());
        }

        private void UpdateContentPublications(ICollection<DynamicContentPublication> original, ICollection<DynamicContentPublication> backup)
        {
            var toUpdate = new List<DynamicContentPublication>();

            backup.CompareTo(original, EqualityComparer<DynamicContentPublication>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _dynamicContentService.CreatePublication(x);
                        break;
                }
            });
            _dynamicContentService.UpdatePublications(toUpdate.ToArray());
        }

        private void UpdateContentFolders(ICollection<DynamicContentFolder> original, ICollection<DynamicContentFolder> backup)
        {
            backup.CompareTo(original, EqualityComparer<DynamicContentFolder>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        _dynamicContentService.UpdateFolder(x);
                        break;
                    case EntryState.Added:
                        _dynamicContentService.CreateFolder(x);
                        break;
                }
            });
        }

        #endregion

        #region BackupObject

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
	        var result = new BackupObject();
            var progressInfo = new ExportImportProgressInfo { Description = "Search promotions..." };
            progressCallback(progressInfo);
			var allPromotions = _marketingSearchService.SearchResources(new MarketingSearchCriteria
            {
                Count = int.MaxValue,
                ResponseGroup = SearchResponseGroup.WithPromotions
            }).Promotions;

			progressInfo.Description = String.Format("{0} promotions loading...", allPromotions.Count());
            progressCallback(progressInfo);
			result.Promotions = allPromotions.Select(x=> _promotionService.GetPromotionById(x.Id)).ToList();

			progressInfo.Description = "Search dynamic content objects...";
			 progressCallback(progressInfo);

            var searchResult = SearchInFolder(null);
            var allFolderSearchResults = searchResult != null ? searchResult.Traverse(ChildrenForFolder).ToArray() : null;


            if (allFolderSearchResults != null)
            {
				progressInfo.Description = String.Format("Loading folders...");
				progressCallback(progressInfo);
				result.ContentFolders = allFolderSearchResults.SelectMany(x => x.ContentFolders).ToList();

				progressInfo.Description = String.Format("Loading places...");
				progressCallback(progressInfo);
                result.ContentPlaces = allFolderSearchResults.SelectMany(x => x.ContentPlaces)
															 .Select(x => _dynamicContentService.GetPlaceById(x.Id))
															 .ToList();

				progressInfo.Description = String.Format("Loading contents...");
				progressCallback(progressInfo);
                result.ContentItems = allFolderSearchResults.SelectMany(x => x.ContentItems)
														    .Select(x => _dynamicContentService.GetContentItemById(x.Id))
															.ToList();
	
				progressInfo.Description = String.Format("Loading publications...");
				progressCallback(progressInfo);
				result.ContentPublications = allFolderSearchResults.SelectMany(x => x.ContentPublications)
																   .Select(x => _dynamicContentService.GetPublicationById(x.Id))
																   .ToList();
            }
            return result;
        }

		private IEnumerable<MarketingSearchResult> ChildrenForFolder(MarketingSearchResult result)
        {
            return result != null && result.ContentFolders != null
				? result.ContentFolders.Select(x => SearchInFolder(x.Id))
                : null;
        }

		private MarketingSearchResult SearchInFolder(string folderId)
        {
            return _marketingSearchService.SearchResources(new MarketingSearchCriteria
            {
				FolderId = folderId,
                Count = int.MaxValue,
                ResponseGroup = SearchResponseGroup.WithContentItems | SearchResponseGroup.WithContentPlaces | SearchResponseGroup.WithContentPublications | SearchResponseGroup.WithFolders
            });
        }

        #endregion

    }
}