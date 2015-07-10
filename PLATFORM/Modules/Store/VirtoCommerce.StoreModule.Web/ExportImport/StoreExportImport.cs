using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.StoreModule.Web.ExportImport
{
    public sealed class StoreExportImport : JsonExportImport
    {
        private readonly IStoreService _storeService;

        public StoreExportImport(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public void DoExport(Stream backupStream, string storeId, Action<ExportImportProgressInfo> progressCallback )
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var store = _storeService.GetById(storeId);

            store.Id = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).GenerateSlug();
            store.PaymentMethods = store.PaymentMethods.Where(x => x.IsActive).ToList();
            store.ShippingMethods = store.ShippingMethods.Where(x => x.IsActive).ToList();
            store.PaymentMethods.ForEach(x => x.Id = null);
            store.ShippingMethods.ForEach(x => x.Id = null);
            store.SeoInfos.ForEach(x => x.Id = null);

            Save(backupStream, store, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, string storeId, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var store = Load<Store>(backupStream, progressCallback, prodgressInfo);
            _storeService.Create(store);
        }

    }
}