using System;
using System.Security.Cryptography;
using System.Text;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services
{
    /// <summary>
    /// SHA-256, lowercase hex. A generated key carries 256 random bits, so a fast hash is enough: a slow
    /// password hash would protect nothing more and would cost CPU on every API request.
    /// </summary>
    public class Sha256ApiKeyHasher : IApiKeyHasher
    {
        public const string HintSeparator = "…";

        private const int HintPartLength = 4;

        public virtual string Hash(string apiKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey);

            // Hex, not Base64: CacheKey.Normalize lower-cases cache keys, and the hash is one.
            return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(apiKey)));
        }

        public virtual string GetHint(string apiKey)
        {
            // Eight characters of a key shorter than 16 would give away half of it or more.
            if (string.IsNullOrEmpty(apiKey) || apiKey.Length < HintPartLength * 4)
            {
                return HintSeparator;
            }

            return apiKey[..HintPartLength] + HintSeparator + apiKey[^HintPartLength..];
        }
    }
}
