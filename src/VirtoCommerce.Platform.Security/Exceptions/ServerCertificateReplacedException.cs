using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Security.Exceptions
{
    public class ServerCertificateReplacedException : PlatformException
    {
        public ServerCertificateReplacedException(string message) : base(message)
        {
        }

        public ServerCertificateReplacedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
