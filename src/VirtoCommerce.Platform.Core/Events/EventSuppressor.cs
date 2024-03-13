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

        private static readonly AsyncLocal<bool> _eventsSuppressedStorage = new();

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static bool EventsSuppressed => _eventsSuppressedStorage.Value;

        [Obsolete("Use SuppressEvents()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public static IDisposable SupressEvents() => SuppressEvents();

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static IDisposable SuppressEvents()
        {
            _eventsSuppressedStorage.Value = true;
            return new DisposableActionGuard(() => { _eventsSuppressedStorage.Value = false; });
        }
    }
}
