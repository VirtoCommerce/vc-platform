using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
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

            var backupObject = new BackupObject
            {
                Pricelists = _pricingService.GetPriceLists().Select(x => x.Id).AsParallel().WithDegreeOfParallelism(4)
                                .Select(x => _pricingService.GetPricelistById(x)).ToArray()
            };

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalPricelists = _pricingService.GetPriceLists().Select(x => x.Id).AsParallel().WithDegreeOfParallelism(4)
                .Select(x => _pricingService.GetPricelistById(x)).ToArray();

            var toUpdate = new List<Pricelist>();

            backupObject.Pricelists.CompareTo(originalPricelists, AnonymousComparer.Create((Pricelist x) => x.Id), (state, x, y) =>
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

    }
}