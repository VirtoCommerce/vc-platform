using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.StoreModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Store> Stores { get; set; }
    }

    public sealed class StoreExportImport
    {
        private readonly IStoreService _storeService;

        public StoreExportImport(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = GetBackupObject();
            //Remove from backup non active methods
            backupObject.Stores.ForEach(x => x.PaymentMethods = x.PaymentMethods.Where(s => s.IsActive).ToList());
            backupObject.Stores.ForEach(x => x.ShippingMethods = x.ShippingMethods.Where(s => s.IsActive).ToList());

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject();

            UpdateStores(originalObject.Stores, backupObject.Stores);
        }

        private void UpdateStores(ICollection<Store> original, ICollection<Store> backup)
        {
            var toUpdate = new List<Store>();

            backup.CompareTo(original, EqualityComparer<Store>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _storeService.Create(x);
                        break;
                }
            });
            _storeService.Update(toUpdate.ToArray());
        }

        private BackupObject GetBackupObject()
        {
            return new BackupObject
            {
                Stores = _storeService.GetStoreList().Select(x => x.Id).Select(x => _storeService.GetById(x)).ToArray()
            };
        }

    }
}