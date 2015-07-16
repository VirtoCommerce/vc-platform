using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

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

            var pricelistIds = _pricingService.GetPriceLists().AsParallel().Select(x => x.Id).ToArray();

            var backupObject = new BackupObject { Pricelists = pricelistIds.AsParallel().Select(x => _pricingService.GetPricelistById(x)).ToArray() };

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var sync = new Object();
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();

            var importedPricelistIds = backupObject.Pricelists.Select(x => x.Id).ToArray();
            var originalPricelistIds = _pricingService.GetPriceLists().Select(x => x.Id).ToArray();

            // create & update
            Parallel.ForEach(backupObject.Pricelists, importedPricelist =>
            {
                var originalPricelistId = originalPricelistIds.FirstOrDefault(x => x == importedPricelist.Id);
                if (originalPricelistId == null)
                {
                    // created new first Pricelist then assugments
                    _pricingService.CreatePricelist(importedPricelist);
                    importedPricelist.Assignments.AsParallel()
                        .ForEach(x => _pricingService.CreatePriceListAssignment(x));
                }
                else
                {
                    // first assignments then pricelist
                    var originalAssignmentIds = _pricingService.GetPricelistById(originalPricelistId).Assignments.Select(x=>x.Id).ToArray();
                    UpdateAssignments(originalAssignmentIds, importedPricelist.Assignments);

                    _pricingService.UpdatePricelists(new[] { importedPricelist });
                }
            });

            // remove old (assignments removes automaticly)
            var pricelistIdsToRemove = originalPricelistIds.Except(importedPricelistIds).ToArray();
            _pricingService.DeletePricelists(pricelistIdsToRemove);
        }

        private void UpdateAssignments(ICollection<string> originalAssignmentIds, ICollection<PricelistAssignment> importedAssignments)
        {
            var toAdd = importedAssignments.Where(x => !originalAssignmentIds.Contains(x.Id)).ToArray();
            foreach (var importedAssignment in toAdd)
            {
                _pricingService.CreatePriceListAssignment(importedAssignment);
            }

            var toUpdate = importedAssignments.Where(x => originalAssignmentIds.Contains(x.Id)).ToArray();
            _pricingService.UpdatePricelistAssignments(toUpdate);

            var importedAssignmenttIds = importedAssignments.Select(x => x.Id).ToArray();
            var toDelete = originalAssignmentIds.Except(importedAssignmenttIds).ToArray();
            _pricingService.DeletePricelistsAssignments(toDelete);
        }

    }
}