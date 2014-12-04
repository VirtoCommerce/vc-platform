using System;
using System.Threading;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class ImportJobRun
    {
        public ImportJobRun()
        {
            id = Guid.NewGuid().ToString();
        }

        public string id;
        public string jobId;
        public string jobName;
        public string assetPath;
        public CancellationTokenSource cancellationTokenSource;
        // public Task task;

        // public string status;
        // public bool cancel;
        // public List<string> errors;
    }
}
