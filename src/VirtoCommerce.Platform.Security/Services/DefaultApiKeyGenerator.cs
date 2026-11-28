using System.Buffers.Text;
using System.Security.Cryptography;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services;

/// <summary>
/// <c>vc_</c> followed by 32 random bytes in Base64Url: 256 bits nobody can guess, and a prefix that
/// secret scanners can match when a key leaks into a repository or a log.
/// </summary>
public class DefaultApiKeyGenerator : IApiKeyGenerator
{
    public const string Prefix = "vc_";

    private const int KeySizeInBytes = 32;

    public virtual string Generate()
    {
        return Prefix + Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(KeySizeInBytes));
    }
}
