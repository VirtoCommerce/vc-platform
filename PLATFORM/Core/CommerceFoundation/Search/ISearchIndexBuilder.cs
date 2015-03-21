using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchIndexBuilder
    {
        string DocumentType { get; }
        IEnumerable<IMessage> CreatePartitions(string scope, DateTime lastBuild);
        IEnumerable<IDocument> CreateDocuments<T>(T input);
        void PublishDocuments(string scope, IDocument[] documents);
        void RemoveDocuments(string scope, string[] documents);
    }
}
