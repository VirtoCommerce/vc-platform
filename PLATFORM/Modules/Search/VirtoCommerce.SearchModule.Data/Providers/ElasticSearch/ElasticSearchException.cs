using System;
using VirtoCommerce.Domain.Search;

namespace VirtoCommerce.SearchModule.Data.Provides.Elastic
{
    /// <summary>
    /// General Elastic Search Exception
    /// </summary>
    public class ElasticSearchException : SearchException
    {

        public ElasticSearchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ElasticSearchException(string message)
            : base(message)
        {
        }

    }
}
