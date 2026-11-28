using System;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class LastChangesCacheRegion : CancellableCacheRegion<LastChangesCacheRegion>
    {
        private static readonly string _tokenKeyPrefix = GenerateRegionTokenKey(string.Empty);

        static LastChangesCacheRegion()
        {
            // Resets made on other instances arrive as CancelForKey(tokenKey) through the Redis backplane.
            TokenCancelled += (_, args) =>
            {
                if (args.TokenKey?.StartsWith(_tokenKeyPrefix, StringComparison.OrdinalIgnoreCase) == true)
                {
                    EntityNameReset?.Invoke(args.TokenKey[_tokenKeyPrefix.Length..]);
                }
            };
        }

        /// <summary>
        /// Raised with the entity name when another platform instance resets it.
        /// </summary>
        internal static event Action<string> EntityNameReset;
    }
}
