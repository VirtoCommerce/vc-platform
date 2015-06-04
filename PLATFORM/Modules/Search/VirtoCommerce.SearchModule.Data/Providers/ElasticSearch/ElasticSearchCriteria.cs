using System.Text;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Data.Providers.ElasticSearch
{
    public class ElasticSearchCriteria : SearchCriteriaBase
    {
        private string _RawQuery;
        /// <summary>
        /// Gets or sets the indexes of the search.
        /// </summary>
        /// <value>
        /// The index of the search.
        /// </value>
        public virtual string RawQuery
        {
            get { return _RawQuery; }
            set { ChangeState(); _RawQuery = value; }
        }

        /// <summary>
        /// Gets the cache key. Used to generate hash that will be used to store data in memory if needed.
        /// </summary>
        /// <value>The cache key.</value>
        public override string CacheKey
        {
            get
            {
                StringBuilder key = new StringBuilder();

                if (this.RawQuery != null)
                {
                    key.Append("_qry:" + RawQuery);
                }

                return base.CacheKey + key.ToString();
            }
        }

        public ElasticSearchCriteria(string documentType)
            : base(documentType)
        {
        }
    }
}
