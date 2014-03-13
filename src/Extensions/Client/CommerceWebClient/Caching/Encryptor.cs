using System.Text;
using System.Web.Security;
using IEncryptor = VirtoCommerce.Web.Client.Caching.Interfaces.IEncryptor;

namespace VirtoCommerce.Web.Client.Caching
{
    public class Encryptor : IEncryptor
    {
        public string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return MachineKey.Encode(plainTextBytes, MachineKeyProtection.Encryption);
        }

        public string Decrypt(string encryptedText)
        {
            var decryptedBytes = MachineKey.Decode(encryptedText, MachineKeyProtection.Encryption);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
