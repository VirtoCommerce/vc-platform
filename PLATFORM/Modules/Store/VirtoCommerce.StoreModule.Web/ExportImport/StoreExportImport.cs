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

            var backupObject = new BackupObject { Stores = _storeService.GetStoreList().ToArray() };
            
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
            var originalStores = _storeService.GetStoreList().Select(x => x.Id).AsParallel().WithDegreeOfParallelism(4)
                .Select(x => _storeService.GetById(x)).ToArray();

            var toUpdate = new List<Store>();

            backupObject.Stores.CompareTo(originalStores, AnonymousComparer.Create((Store x) => x.Id), (state, x, y) =>
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

        //private void UpdateSettings(ICollection<SettingEntry> originalSettings, ICollection<SettingEntry> importedSettings)
        //{
        //    Parallel.ForEach(importedSettings, importedSetting =>
        //    {
        //        var originalSetting = originalSettings.FirstOrDefault(x => x.Name == importedSetting.Name);
        //        importedSetting.ObjectId = originalSetting != null ? originalSetting.ObjectId : null;
        //    });
        //}

    }
}