using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common.Exceptions
{
    public class StorefrontException : Exception
    {
        public StorefrontException(string message)
            : base(message)
        {
        }

        public StorefrontException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
