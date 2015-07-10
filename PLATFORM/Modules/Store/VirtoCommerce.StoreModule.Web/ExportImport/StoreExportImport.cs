using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.StoreModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Store> Stores { get; set; }
    }

    public sealed class StoreExportImport : JsonExportImport
    {
        private readonly IStoreService _storeService;

        public StoreExportImport(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public void DoExport(Stream backupStream, BackupObject backupObject, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            backupObject.Stores.ForEach(x => x.PaymentMethods = x.PaymentMethods.Where(s => s.IsActive).ToList());
            backupObject.Stores.ForEach(x => x.ShippingMethods = x.ShippingMethods.Where(s => s.IsActive).ToList());

            Save(backupStream, backupObject, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            //var backupObject = Load<BackupObject>(backupStream, progressCallback, prodgressInfo);
            //foreach (var store in backupObject.Stores)
            //{
            //    var originalStore = _storeService.GetById(store.Id);
            //    if (originalStore == null)
            //    {
            //        _storeService.Create(store);
            //    }
            //    else
            //    {
            //        originalStore.InjectFrom(store);
            //        _storeService.Update(new[] { originalStore });
            //    }
            //}
        }

    }
}