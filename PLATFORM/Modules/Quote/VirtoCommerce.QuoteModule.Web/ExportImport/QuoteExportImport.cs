using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.QuoteModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<QuoteRequest> QuoteRequests { get; set; }
    }

    public sealed class QuoteExportImport
    {
        private readonly IQuoteRequestService _quoteRequestService;

        public QuoteExportImport(IQuoteRequestService quoteRequestService)
        {
            _quoteRequestService = quoteRequestService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = String.Format("{0} RFQs importing", backupObject.QuoteRequests.Count());
            progressCallback(progressInfo);
            _quoteRequestService.SaveChanges(backupObject.QuoteRequests.ToArray());
        }

      

        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            var retVal = new BackupObject();
            var progressInfo = new ExportImportProgressInfo();
       
            var searchResponse = _quoteRequestService.Search(new QuoteRequestSearchCriteria { Count = int.MaxValue });

            progressInfo.Description = String.Format("{0} RFQs loading", searchResponse.QuoteRequests.Count());
            progressCallback(progressInfo);

            retVal.QuoteRequests = _quoteRequestService.GetByIds(searchResponse.QuoteRequests.Select(x => x.Id).ToArray()).ToList();

            return retVal;
        }

    }
}