using System;
using System.Threading;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class CacheDisabler
    {
        private sealed class DisposableActionGuard : IDisposable
        {
            private readonly Action _action;

            public DisposableActionGuard(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }

            public void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _action();
                }
            }
        }

        private static readonly AsyncLocal<bool> CacheDisablerStorage = new();

        public static bool CacheDisabled => CacheDisablerStorage.Value;

        /// <summary>
        /// The method disables caching in current and inherited threads and set up
        /// enabling of it as callback action wich runs when returting object is disposed.
        /// </summary>
        /// <returns>Disposable object wich enables cache back on disposing</returns>
        public static IDisposable DisableCache()
        {
            CacheDisablerStorage.Value = true;
            return new DisposableActionGuard(() => { CacheDisablerStorage.Value = false; });
        }
    }
}
