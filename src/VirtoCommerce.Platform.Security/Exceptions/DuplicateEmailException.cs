using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Security.Exceptions
{
    public class DuplicateEmailException : PlatformException
    {
        public DuplicateEmailException(string email)
            : base($"Multiple accounts are associated with the email '{email}'.")
        {
        }

        public DuplicateEmailException(string email, Exception innerException)
            : base($"Multiple accounts are associated with the email '{email}'.", innerException)
        {
        }
    }
}
