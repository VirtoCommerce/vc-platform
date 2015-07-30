using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Inventory.Model;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.InventoryModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public InventoryInfo[] InventoryInfos { get; set; }
    }

    public sealed class InventoryExportImport
    {
        private readonly IInventoryService _inventoryService;

        public InventoryExportImport(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }


        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
            progressCallback(new ExportImportProgressInfo("export inventory done"));
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var originalObject = GetBackupObject(progressCallback);
            var backupObject = backupStream.DeserializeJson<BackupObject>();

            UpdateInventories(originalObject.InventoryInfos, backupObject.InventoryInfos);
            progressCallback(new ExportImportProgressInfo("update inventory done"));
        }

        private void UpdateInventories(InventoryInfo[] original, InventoryInfo[] backup)
        {
            var toUpdate = new List<InventoryInfo>();

            backup.CompareTo(original, EqualityComparer<InventoryInfo>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _inventoryService.Create(x);
                        break;
                }
            });
            _inventoryService.UpsertInventories(toUpdate.ToArray());
        }

        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            progressCallback(new ExportImportProgressInfo("inventories loading"));
            var inventoryInfos = _inventoryService.GetAllInventoryInfos().ToArray();
            progressCallback(new ExportImportProgressInfo(string.Format("inventories loaded: {0}", inventoryInfos.Count())));

            return new BackupObject()
            {
                InventoryInfos = inventoryInfos
            };
        }
    }
}