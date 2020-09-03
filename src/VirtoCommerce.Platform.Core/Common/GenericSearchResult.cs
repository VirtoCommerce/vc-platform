using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    public class GenericSearchResult<T>
    {
        public GenericSearchResult()
        {
            Results = new List<T>();
        }
        public int TotalCount { get; set; }
        public IList<T> Results { get; set; }
    }
}
