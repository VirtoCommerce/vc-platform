using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Search.Providers.Azure
{
    using VirtoCommerce.Foundation.Search;

    public class AzureSearchException : SearchException
    {
        public AzureSearchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AzureSearchException(string message)
            : base(message)
        {
        }
    }
}
