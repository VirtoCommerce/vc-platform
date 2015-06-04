using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchIndexDocumentCreator<in T>
    {
        IEnumerable<IDocument> CreateDocument(T source);
    }
}
