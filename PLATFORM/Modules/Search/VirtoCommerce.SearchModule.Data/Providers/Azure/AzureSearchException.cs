using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Search;

namespace VirtoCommerce.SearchModule.Data.Provides.Azure
{

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
