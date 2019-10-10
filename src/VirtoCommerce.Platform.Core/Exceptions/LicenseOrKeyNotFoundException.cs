using System;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    public class LicenseOrKeyNotFoundException : PlatformException
    {
        public LicenseOrKeyNotFoundException(string path) : base($"The License or Public Key {path} aren't found.")
        {
            
        }

        public LicenseOrKeyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
