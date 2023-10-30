using System;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    public class InvalidCollectionItemException : Exception
    {
        public InvalidCollectionItemException() { }

        public InvalidCollectionItemException(string message) : base(message) { }

        public InvalidCollectionItemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
