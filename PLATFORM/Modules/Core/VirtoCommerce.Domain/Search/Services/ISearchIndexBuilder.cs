using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchIndexBuilder
    {
        string DocumentType { get; }
        IEnumerable<Partition> GetPartitions(string scope, DateTime lastBuild);
        IEnumerable<IDocument> CreateDocuments(Partition partition);
        void PublishDocuments(string scope, IDocument[] documents);
        void RemoveDocuments(string scope, string[] documents);
    }
}
