using System;
using System.Threading;

namespace VirtoCommerce.Platform.Core.Events
{
    /// <summary>
    /// The static class used to suppress emitting all domain events for the current asynchronous control flow
    /// </summary>
    public static class EventSuppressor
    {
        private class DisposableActionGuard : IDisposable
        {
            private readonly Action _action;

            public DisposableActionGuard(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _action();
                }
            }
        }

        private static readonly AsyncLocal<bool> EventsSuppressedStorage = new AsyncLocal<bool>();

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static bool EventsSuppressed => EventsSuppressedStorage.Value;

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static IDisposable SupressEvents()
        {
            EventsSuppressedStorage.Value = true;
            return new DisposableActionGuard(() => { EventsSuppressedStorage.Value = false; });
        }
    }
}
