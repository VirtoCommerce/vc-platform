using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// An API key value the platform refuses to store: one that looks masked, or one too short to be safe.
    /// The message is meant for the client that sent the value.
    /// </summary>
    public class ApiKeyValidationException : PlatformException
    {
        public ApiKeyValidationException(string message)
            : base(message)
        {
        }
    }
}
