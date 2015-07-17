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
        public ICollection<PricelistAssignment> Assignments { get; set; }
    }

    public class ExportImport
    {
        static public void Update<T>(ICollection<string> originalIds, ICollection<T> importedCollection, Action<string[]> deleteDelegate, Action<T[]> updateDelegate, Func<T, T> createDelegate) where T : Entity
        {
            var toAdd = importedCollection.Where(x => !originalIds.Contains(x.Id)).ToArray();
            var toUpdate = importedCollection.Where(x => originalIds.Contains(x.Id)).ToArray();
            var toRemove = originalIds.Where(x => importedCollection.All(s => s.Id != x)).ToArray();

            if (toRemove.Any())
            {
                deleteDelegate(toRemove);
            }
            if (toUpdate.Any())
            {
                updateDelegate(toUpdate);
            }
            foreach (var item in toAdd)
            {
                createDelegate(item);
            }
        }
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
            var backupObject = new BackupObject
            {
                Pricelists = pricelistIds.AsParallel().Select(x => _pricingService.GetPricelistById(x)).ToArray()
            };

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();

            var assignments = backupObject.Pricelists.SelectMany(x => x.Assignments).ToArray();
            backupObject.Pricelists.ForEach(x=>x.Assignments =null);

            var originalIds = _pricingService.GetPriceLists().Select(x => x.Id).ToArray();
            ExportImport.Update(originalIds, backupObject.Pricelists,
                _pricingService.DeletePricelists, _pricingService.UpdatePricelists, _pricingService.CreatePricelist);

            originalIds = _pricingService.GetPriceListAssignments().Select(x => x.Id).ToArray();
            ExportImport.Update(originalIds, assignments,
                _pricingService.DeletePricelistsAssignments, _pricingService.UpdatePricelistAssignments, _pricingService.CreatePriceListAssignment);

        }


    }
}