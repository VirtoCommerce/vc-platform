using System;
using System.Collections.Generic;
using System.IO;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.PricingModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Pricelist> Pricelists { get; set; }
    }

    public sealed class PricingExportImport : JsonExportImport
    {
        private readonly IPricingService _pricingService;

        public PricingExportImport(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        public void DoExport(Stream backupStream, BackupObject backupObject, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            Save(backupStream, backupObject, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, string storeId, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            //var backupObject = Load<BackupObject>(backupStream, progressCallback, prodgressInfo);
        }

    }
}