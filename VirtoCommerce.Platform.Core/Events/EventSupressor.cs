using System;
using System.Threading;

namespace VirtoCommerce.Platform.Core.Events
{
    /// <summary>
    /// The static class used to suppress emitting all domain events for the current asynchronous control flow
    /// </summary>
    public static class EventSupressor
    {
        private static AsyncLocal<bool> _eventsSuppressed = new AsyncLocal<bool>();

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static bool EventsSuppressed
        {
            get
            {
                return _eventsSuppressed.Value;
            }
        }

        /// <summary>
        /// The flag indicates that events are suppressed for the current asynchronous control flow
        /// </summary>
        public static DisposibleActionGuard SupressEvents()
        {
            _eventsSuppressed.Value = true;
            return new DisposibleActionGuard(() => { _eventsSuppressed.Value = false; });
        }
    }

    public class DisposibleActionGuard : IDisposable
    {
        private readonly Action _action;
        public DisposibleActionGuard(Action action)
        {
            _action = action;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _action();
                // free managed resources
            }
            // free native resources if there are any.
        }
    }
}
