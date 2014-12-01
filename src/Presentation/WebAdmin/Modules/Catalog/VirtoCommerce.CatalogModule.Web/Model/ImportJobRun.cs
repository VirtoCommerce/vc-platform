using System;
using System.Collections.Generic;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class ImportJobRun
    {
        public ImportJobRun()
        {
            id = Guid.NewGuid().ToString();
            status = "Submitted";
            errors = new List<string>();
        }

        public string id;
        public string jobId;
        public string AssetPath;
        // public Task task;

        public string status;
        public bool cancel;
        public List<string> errors;
    }
}
