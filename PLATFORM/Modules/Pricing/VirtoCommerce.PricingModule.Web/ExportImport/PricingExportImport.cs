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
        static public void Update<T>(ICollection<string> originalIds, ICollection<T> importedCollection, Action<string[]> deleteDelegate, Action<T[]> updateDelegate, Action<T> createDelegate) where T : Entity
        {
            var toAdd = importedCollection.Where(x => !originalIds.Contains(x.Id)).ToArray();
            var toUpdate = importedCollection.Where(x => originalIds.Contains(x.Id)).ToArray();
            var toRemove = originalIds.Where(x => importedCollection.All(s => s.Id != x)).ToArray();

            if (toRemove.Any()) deleteDelegate(toRemove);
            if (toUpdate.Any()) updateDelegate(toUpdate);
            toAdd.ForEach(createDelegate);
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

            var pricelists = _pricingService.GetPriceLists().ToArray();
            pricelists.ForEach(x=>x.Assignments.Clear());

            var backupObject = new BackupObject
            {
                Pricelists = pricelists,
                Assignments = _pricingService.GetPriceListAssignments().ToArray(),
            };

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            
            var originalIds = _pricingService.GetPriceLists().Select(x => x.Id).ToArray();
            //ExportImport.Update(originalIds, backupObject.Pricelists, _pricingService.DeletePricelists, _pricingService.UpdatePricelists,
            //    pricelist => _pricingService.CreatePricelist(pricelist));
            UpdatePriceList(originalIds, backupObject.Pricelists);
            
            originalIds = _pricingService.GetPriceLists().Select(x => x.Id).ToArray();
            UpdateAssignments(originalIds, backupObject.Assignments);
        }

        private void UpdatePriceList(ICollection<string> originalIds, ICollection<Pricelist> importedPricelist)
        {
            var toAdd = importedPricelist.Where(x => !originalIds.Contains(x.Id)).ToArray();
            var toUpdate = importedPricelist.Where(x => originalIds.Contains(x.Id)).ToArray();
            var toRemove = originalIds.Where(x => importedPricelist.All(s => s.Id != x)).ToArray();

            if(toRemove.Any()) _pricingService.DeletePricelists(toRemove);
            if(toUpdate.Any()) _pricingService.UpdatePricelists(toUpdate);
            toAdd.ForEach(x => _pricingService.CreatePricelist(x));
        }

        private void UpdateAssignments(ICollection<string> originalIds, ICollection<PricelistAssignment> importedAssignments)
        {
            var toAdd = importedAssignments.Where(x => !originalIds.Contains(x.Id)).ToArray();
            var toUpdate = importedAssignments.Where(x => originalIds.Contains(x.Id)).ToArray();
            var toRemove = originalIds.Where(x => importedAssignments.All(s => s.Id != x)).ToArray();

            if (toRemove.Any()) _pricingService.DeletePricelistsAssignments(toRemove);
            if (toUpdate.Any()) _pricingService.UpdatePricelistAssignments(toUpdate);
            toAdd.ForEach(x => _pricingService.CreatePriceListAssignment(x));
        }

    }
}