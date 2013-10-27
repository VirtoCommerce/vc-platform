using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Search.Providers.Elastic
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
