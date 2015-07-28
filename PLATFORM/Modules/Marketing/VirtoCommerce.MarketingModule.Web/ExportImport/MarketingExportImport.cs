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
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = GetBackupObject();
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject();

            UpdateContentFolders(originalObject.ContentFolders, backupObject.ContentFolders);
            UpdateContentPlaces(originalObject.ContentPlaces, backupObject.ContentPlaces);
            UpdateContentItems(originalObject.ContentItems, backupObject.ContentItems);
            UpdateContentPublications(originalObject.ContentPublications, backupObject.ContentPublications);

            //UpdateCoupons(originalObject.Coupons, backupObject.Coupons);
            UpdatePromotions(originalObject.Promotions, backupObject.Promotions);
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
        
        private BackupObject GetBackupObject()
        {
            var result = new BackupObject();
            
            var rootFolder = GetMarketingSearchResult(null);
            var folders = rootFolder != null ? rootFolder.Traverse(ChildrenForContent).ToArray() : null;

            if (folders != null)
            {
                result.Promotions = folders.SelectMany(x => x.Promotions)
                    .Select(x => _promotionService.GetPromotionById(x.Id)).ToArray();
                result.ContentPublications = folders.SelectMany(x => x.ContentPublications)
                    .Select(x => _dynamicContentService.GetPublicationById(x.Id)).ToArray();
                result.Coupons = folders.SelectMany(x => x.Coupons).ToArray();
                result.ContentPlaces = folders.SelectMany(x => x.ContentPlaces).ToArray();
                result.ContentItems = folders.SelectMany(x => x.ContentItems).ToArray();
                result.ContentFolders = folders.SelectMany(x => x.ContentFolders).ToArray();
            }
            return result;
        }

        private IEnumerable<MarketingSearchResult> ChildrenForContent(MarketingSearchResult result)
        {
            return result != null && result.ContentFolders != null
                ? result.ContentFolders.Select(x => GetMarketingSearchResult(x.Id))
                : null;
        }

        private MarketingSearchResult GetMarketingSearchResult(string id)
        {
            return _marketingSearchService.SearchResources(new MarketingSearchCriteria
            {
                FolderId = id,
                Count = int.MaxValue,
                ResponseGroup = SearchResponseGroup.WithFolders | SearchResponseGroup.WithContentItems
                    | SearchResponseGroup.WithContentPlaces
            });
        }

        #endregion

    }
}