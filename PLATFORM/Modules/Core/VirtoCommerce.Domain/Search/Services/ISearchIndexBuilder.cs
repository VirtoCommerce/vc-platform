using System;
using System.Collections.Generic;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchIndexBuilder
    {
        string DocumentType { get; }
        IEnumerable<Partition> GetPartitions(bool rebuild, DateTime startDate, DateTime endDate);
        IEnumerable<IDocument> CreateDocuments(Partition partition);
        void PublishDocuments(string scope, IDocument[] documents);
        void RemoveDocuments(string scope, string[] documents);
        void RemoveAll(string scope);
    }
}
