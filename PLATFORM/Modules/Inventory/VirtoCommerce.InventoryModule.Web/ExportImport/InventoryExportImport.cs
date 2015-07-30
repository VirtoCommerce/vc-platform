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
            var backupObject = backupStream.DeserializeJson<BackupObject>();

            _inventoryService.UpsertInventories(backupObject.InventoryInfos);
            progressCallback(new ExportImportProgressInfo("update inventory done"));
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