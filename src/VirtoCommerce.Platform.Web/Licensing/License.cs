using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public sealed class License
    {
        private static readonly string _hashAlgorithmName = HashAlgorithmName.SHA256.Name;

        public string Type { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RawLicense { get; set; }

        public static License Parse(string rawLicense, string publicKeyPath)
        {
            License result = null;

            if (!string.IsNullOrEmpty(rawLicense))
            {
                using (var reader = new StringReader(rawLicense))
                {
                    var data = reader.ReadLine();
                    var signature = reader.ReadLine();

                    if (data != null && signature != null)
                    {
                        if (ValidateSignature(data, signature, publicKeyPath))
                        {
                            result = JsonConvert.DeserializeObject<License>(data);
                            result.RawLicense = rawLicense;
                        }
                    }
                }
            }

            return result;
        }

        private static bool ValidateSignature(string data, string signature, string publicKeyPath)
        {
            bool result;
            byte[] dataHash;

            var dataBytes = Encoding.UTF8.GetBytes(data);
            using (var algorithm = SHA256.Create())
            {
                dataHash = algorithm.ComputeHash(dataBytes);
            }

            var signatureBytes = Convert.FromBase64String(signature);

            try
            {
#pragma warning disable S4426 // The license intentionally has a low cryptography strength because it wasn’t designed to be hijacked-proof.
                using (var rsa = new RSACryptoServiceProvider())
#pragma warning restore S4426 // The license intentionally has a low cryptography strength because it wasn’t designed to be hijacked-proof.
                {
                    rsa.FromXmlStringCustom(ReadFileWithKey(publicKeyPath));

                    var signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                    signatureDeformatter.SetHashAlgorithm(_hashAlgorithmName);
                    result = signatureDeformatter.VerifySignature(dataHash, signatureBytes);
                }
            }
            catch (FormatException)
            {
                result = false;
            }

            return result;
        }

        private static string ReadFileWithKey(string path)
        {
            if (!File.Exists(path))
            {
                throw new LicenseOrKeyNotFoundException(path);
            }

            return File.ReadAllText(path);
        }
    }
}
