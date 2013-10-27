using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Foundation.Search
{
    public class DocumentPublisher
    {
        readonly ISearchProvider _search = null;
        public DocumentPublisher(ISearchProvider search)
        {
            _search = search;
        }

        public void SubmitDocuments(string scope, string documentType, IDocument[] documents)
        {
            foreach (var doc in documents)
            {
                _search.Index(scope, documentType, doc);
            }

            _search.Commit(scope);

            /*
            var criteria = new CatalogItemSearchCriteria();
            var results = _search.Search(scope, criteria);
            Trace.TraceInformation("Total items : " + results.TotalCount);
             * */
        }

        public void RemoveDocuments(string scope, string documentType, string[] documents)
        {
            foreach (var doc in documents)
            {
                _search.Remove(scope, documentType, "__key", doc);
            }

            _search.Commit(scope);
        }
    }
}
