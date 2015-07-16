using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Omu.ValueInjecter;
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

            var backupObject = new BackupObject { Pricelists = pricelistIds.AsParallel().Select(x=>_pricingService.GetPricelistById(x)).ToArray() };

            backupStream.JsonSerializationObject(backupObject, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var sync = new Object();
            var pricelistsForUpdate = new List<Pricelist>();

            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.JsonDeserializationObject<BackupObject>(progressCallback, prodgressInfo);

            foreach (var importedPricelist in backupObject.Pricelists)
            {
                var originalPricelist = _pricingService.GetPricelistById(importedPricelist.Id);//todo maybe by name?
                if (originalPricelist == null)
                {
                    _pricingService.CreatePricelist(importedPricelist); //todo maybe ids could conflict with existing items
                }
                else
                {
                    UpdatePrices(originalPricelist.Prices, importedPricelist.Prices);
                    UpdatePriceAssignments(originalPricelist.Assignments, importedPricelist.Assignments);

                    originalPricelist.InjectFrom(importedPricelist);
                    lock (sync) 
                    {
                        pricelistsForUpdate.Add(originalPricelist);
                    }
                }
            }

            if (pricelistsForUpdate.Any())
            {
                _pricingService.UpdatePricelists(pricelistsForUpdate.ToArray());
            }
        }

        private void UpdatePrices(ICollection<Price> originalPrices, ICollection<Price> importedPrices)
        {
            importedPrices.ForEach((x) => { x.PricelistId = null; });

            Parallel.ForEach(importedPrices, importedPrice =>
            {
                var originalPrice = originalPrices.FirstOrDefault(x => x.ProductId == importedPrice.ProductId);
                if (originalPrice != null)
                {
                    importedPrice.Id = originalPrice.Id;
                }
            });
        }

        private void UpdatePriceAssignments(ICollection<PricelistAssignment> originalPrices, ICollection<PricelistAssignment> importedPrices)
        {
            importedPrices.ForEach((x) => { x.PricelistId = null; });

            Parallel.ForEach(importedPrices, importedPrice =>
            {
                var originalPrice = originalPrices.FirstOrDefault(x => x.CatalogId == importedPrice.CatalogId);
                if (originalPrice != null)
                {
                    importedPrice.Id = originalPrice.Id;
                }
            });
        }

    }
}