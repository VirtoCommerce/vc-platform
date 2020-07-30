using System;

namespace VirtoCommerce.Platform.Core.Caching
{
    public sealed class TokenCancelledEventArgs : EventArgs
    {
        public TokenCancelledEventArgs(string tokenKey)
        {
            TokenKey = tokenKey;
        }

        public string TokenKey { get; }
        public override string ToString()
        {
            return $"{TokenKey}";
        }
    }
}
