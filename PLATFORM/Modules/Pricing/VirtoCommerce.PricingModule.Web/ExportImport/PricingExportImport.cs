using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.PricingModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Pricelist> Pricelists { get; set; }
    }

    public sealed class PricingExportImport
    {
        private readonly IPricingService _pricingService;

        public PricingExportImport(IPricingService pricingService)
        {
            _pricingService = pricingService;
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

            UpdatePricelist(originalObject.Pricelists, backupObject.Pricelists);
        }

        private void UpdatePricelist(ICollection<Pricelist> original, ICollection<Pricelist> backup)
        {
            var toUpdate = new List<Pricelist>();

            backup.CompareTo(original, EqualityComparer<Pricelist>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _pricingService.CreatePricelist(x);
                        break;
                }
            });
            _pricingService.UpdatePricelists(toUpdate.ToArray());
        }

        private BackupObject GetBackupObject()
        {
            return new BackupObject
            {
                Pricelists = _pricingService.GetPriceLists().Select(x => x.Id).Select(x => _pricingService.GetPricelistById(x)).ToArray()
            };
        }

    }
}