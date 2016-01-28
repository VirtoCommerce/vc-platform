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
			var backupObject = GetBackupObject(progressCallback);
            //Remove from backup non active methods
            backupObject.Stores.ForEach(x => x.PaymentMethods = x.PaymentMethods.Where(s => s.IsActive).ToList());
            backupObject.Stores.ForEach(x => x.ShippingMethods = x.ShippingMethods.Where(s => s.IsActive).ToList());

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback);

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} stores importing...", backupObject.Stores.Count());
			progressCallback(progressInfo);

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

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo("stores loading...");
			progressCallback(progressInfo);

            return new BackupObject
            {
                Stores = _storeService.SearchStores(new SearchCriteria { Take = int.MaxValue }).Stores
            };
        }

    }
}