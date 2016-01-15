using System;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CoreModule.Web.ExportImport
{
	public sealed class BackupObject
	{
		public FulfillmentCenter[] FulfillmentCenters { get; set; }
        public Currency[] Currencies { get; set; }
	}

	public sealed class CoreExportImport
	{
		private readonly ICommerceService _commerceService;

		public CoreExportImport(ICommerceService commerceService)
		{
			_commerceService = commerceService;
		}

		public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = GetBackupObject(progressCallback);
			backupObject.SerializeJson(backupStream);
		}

		public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = backupStream.DeserializeJson<BackupObject>();
            progressCallback(new ExportImportProgressInfo("importing fulfillmentCenters"));
            if (backupObject.FulfillmentCenters != null)
            {
                backupObject.FulfillmentCenters.ForEach(x => _commerceService.UpsertFulfillmentCenter(x));
            }
            progressCallback(new ExportImportProgressInfo("imporing currencies"));
            if (backupObject.Currencies != null)
            {
                _commerceService.UpsertCurrencies(backupObject.Currencies);
            }

        }

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
		{
			progressCallback(new ExportImportProgressInfo("fulfillmentCenters loading"));
			var fulfillments = _commerceService.GetAllFulfillmentCenters().ToArray();
			progressCallback(new ExportImportProgressInfo(string.Format("fulfillmentCenters loaded: {0}", fulfillments.Count())));

            progressCallback(new ExportImportProgressInfo("currencies loading"));
            var currencies = _commerceService.GetAllCurrencies().ToArray();
            progressCallback(new ExportImportProgressInfo(string.Format("currencies loaded: {0}", currencies.Count())));
            return new BackupObject()
            {
                FulfillmentCenters = fulfillments,
                Currencies = currencies
			};
		}
	}
}