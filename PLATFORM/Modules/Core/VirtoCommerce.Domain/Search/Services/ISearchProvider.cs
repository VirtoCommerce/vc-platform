using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchProvider
    {
        ISearchQueryBuilder QueryBuilder { get; }

        ISearchResults Search(string scope, ISearchCriteria criteria);

        void Index(string scope, string documentType, IDocument document);

        int Remove(string scope, string documentType, string key, string value);

        void RemoveAll(string scope, string documentType);

        void Close(string scope, string documentType);

        void Commit(string scope);
    }
}
