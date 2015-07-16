using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.MarketingModule.Web.ExportImport
{

    public sealed class BackupObject
    {
        public ICollection<DynamicContentPublication> ContentPublications { get; set; }
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Coupon> Coupons { get; set; }
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

            var responce = _marketingSearchService.SearchResources(new MarketingSearchCriteria { Count = int.MaxValue });
            var backupObject = new BackupObject
            {
                ContentPublications = responce.ContentPublications.Select(x => x.Id).Select(_dynamicContentService.GetPublicationById).ToArray(),
                Promotions = responce.Promotions.Select(x => x.Id).Select(_promotionService.GetPromotionById).ToArray(),
                Coupons = responce.Coupons.Select(x => x.Id).Select(_promotionService.GetCouponById).ToArray()
            };

            backupObject.SerializeJson(backupStream);
        }
    }
}