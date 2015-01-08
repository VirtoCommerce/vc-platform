using System;
using System.Text;
using System.Web.Security;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;

namespace VirtoCommerce.ApiWebClient.Caching
{
    public class Encryptor : IEncryptor
    {
        public string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = MachineKey.Protect(plainTextBytes);
            return Convert.ToBase64String(encryptedBytes);
            //return MachineKey.Encode(plainTextBytes, MachineKeyProtection.Encryption);
        }

        public string Decrypt(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedBytes = MachineKey.Unprotect(encryptedBytes);
            return decryptedBytes != null ? Encoding.UTF8.GetString(decryptedBytes) : encryptedText;
        }
    }
}
