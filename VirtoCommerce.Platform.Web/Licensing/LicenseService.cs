using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Licensing;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseService
    {
        private static readonly string _hashAlgorithmName = HashAlgorithmName.SHA256.Name;
        private static readonly HashAlgorithm _hashAlgorithm = HashAlgorithm.Create(_hashAlgorithmName);
        private static readonly AsymmetricSignatureDeformatter _signatureDeformatter = CreateSignatureDeformatter(_hashAlgorithmName);

        public License LoadLicense(string filePath)
        {
            License result = null;

            var container = ReadLicenseFile(filePath);
            if (container?.Data != null && container.Signature != null)
            {
                if (ValidateSignature(container.Data, container.Signature))
                {
                    result = JsonConvert.DeserializeObject<License>(container.Data);
                }
            }

            return result;
        }


        private static LicenseContainer ReadLicenseFile(string filePath)
        {
            LicenseContainer result = null;

            if (File.Exists(filePath))
            {
                using (var reader = File.OpenText(filePath))
                {
                    result = new LicenseContainer
                    {
                        Data = reader.ReadLine(),
                        Signature = reader.ReadLine(),
                    };
                }
            }

            return result;
        }

        private static bool ValidateSignature(string data, string signature)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var dataHash = _hashAlgorithm.ComputeHash(dataBytes);
            var signatureBytes = Convert.FromBase64String(signature);

            return _signatureDeformatter.VerifySignature(dataHash, signatureBytes);
        }

        private static RSAPKCS1SignatureDeformatter CreateSignatureDeformatter(string hashAlgorithmName)
        {
            var rsa = new RSACryptoServiceProvider();

            // Import public key
            rsa.FromXmlString("<RSAKeyValue><Modulus>uYgtG8GG6fZ4jZdaL6LF4f2vmmTHNr0H/m+Bfo4vNhOYDlUTOv89FVQ3xE0DPhZ2uQ6Q/AN9KausQz2VbdfUn0Ge/jcHNsdE+9SBdllzgvCr/2sUlCKcpiEIBC9AXnAd7lKFSHiS61cVLo24+8aowoeGsAAO3djqN2xP+4Co9CMywKscLSPUMOJWHMuXAr3+pjamYaqwe3/iv5VA/8ff0evVyqhE/8fIixm9Ti7OhPNwYRDmTKP+t4DRZlp4R46g4v43tg4Q9FYaGKRCuxAdbbEsTYhFzHzv/CcUoFzYF0x3lyW5mfqad5y+LhsWPiHGDrd+xWXq9Nho1glNZ0sGYQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

            var signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            signatureDeformatter.SetHashAlgorithm(hashAlgorithmName);

            return signatureDeformatter;
        }
    }
}
