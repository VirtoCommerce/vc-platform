using System;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace Shipstation.FulfillmentModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public FulfillmentCenter[] FulfillmentCenters { get; set; }
    }

    public sealed class FulfillmentExportImport
    {
        private readonly ICommerceService _commerceService;

        public FulfillmentExportImport(ICommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
            progressCallback(new ExportImportProgressInfo("export fulfillmentCenters done"));
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();

            backupObject.FulfillmentCenters.ForEach(x=>_commerceService.UpsertFulfillmentCenter(x));
            progressCallback(new ExportImportProgressInfo("update fulfillmentCenters done"));
        }

        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            progressCallback(new ExportImportProgressInfo("fulfillmentCenters loading"));
            var fulfillments = _commerceService.GetAllFulfillmentCenters().ToArray();
            progressCallback(new ExportImportProgressInfo(string.Format("fulfillmentCenters loaded: {0}", fulfillments.Count())));

            return new BackupObject()
            {
                FulfillmentCenters = fulfillments
            };
        }
    }
}