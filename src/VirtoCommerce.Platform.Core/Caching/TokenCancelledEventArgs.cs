using System;

namespace VirtoCommerce.Platform.Core.Caching
{
    public sealed class TokenCancelledEventArgs : EventArgs
    {
        public TokenCancelledEventArgs(string tokenKey)
            : this(tokenKey, true)
        {
        }
        public TokenCancelledEventArgs(string tokenKey, bool propagate)
        {
            TokenKey = tokenKey;
            Propagate = propagate;
        }

        public string TokenKey { get; }
        public bool Propagate { get;  }

        public override string ToString()
        {
            return $"{TokenKey}";
        }
    }
}
