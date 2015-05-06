using System;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Data.Providers.Azure
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
