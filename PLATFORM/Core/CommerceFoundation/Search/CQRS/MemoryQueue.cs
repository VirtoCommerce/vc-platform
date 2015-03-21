using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VirtoCommerce.Foundation.Search.CQRS
{
    /// <summary>
    /// Implements a generic threadsafe synchronized queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MemoryQueue<T>
    {
        #region Private Variables

        private readonly object _starveRoot = new object();
        private readonly object _syncLock = new object();
        private readonly LinkedList<T> _queue = new LinkedList<T>();

        private volatile bool _releasing;

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        /// Returns the number of objects in the Queue
        /// </summary>
        public int Count
        {
            get { return _queue.Count; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Enqueues the supplied object. Will turn off release mode 
        /// </summary>
        public void Enqueue(T obj)
        {
            // Lock on out lock object
            lock (_syncLock)
            {
                // Set's releasing to false
                _releasing = false;

                // Enqueue
                _queue.AddLast(obj);
                // And let a waiting thread know
                Monitor.Pulse(_syncLock);
            }
        }

        /// <summary>
        /// Dequeues the object at the front of the LinkedList and waits for the specified time.  
        /// If no object becomes available in this time, returns null
        /// </summary>
        /// <param name="timeSpan">TimeSpan object containing amount of time to wait</param>
        public T Dequeue(TimeSpan timeSpan)
        {
            T returnValue = default(T);
            try
            {
                lock (_syncLock)
                {
                    // While there are items in the LinkedList
                    while (Count == 0 && !_releasing)
                    {
                        try
                        {
                            if (!Monitor.Wait(_syncLock, timeSpan))
                                return returnValue;
                        }
                        catch
                        {
                            // If we get interupted or aborted, we may have been pulsed before.
                            // If we just exit, that pulse would get lost and
                            // possibly result in a "live" lock where other threads are waiting
                            // on syncLock, and never get a pulse.
                            // Regenerate a Pulse as we consumed it.  Even if we did not get
                            // pulsed, this does not hurt as any thread will check again for count = 0.
                            lock (_syncLock)
                            {
                                Monitor.Pulse(_syncLock);
                            }
                            // Rethrow the exception for caller.  Now semaphore state is same as if
                            // this call never happened.  Caller must decide how to handle exception.
                            throw;
                        }
                    }

                    // If there is data
                    if (Count > 0)
                    {
                        returnValue = _queue.First.Value;
                        _queue.RemoveFirst();

                        if (Count == 0)
                        {
                            lock (_starveRoot)
                            {
                                Monitor.PulseAll(_starveRoot);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (_releasing)
                {
                    _releasing = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Dequeues the object at the front of the queue and waits for the specified time.  
        /// If no object becomes available in this time, returns null
        /// </summary>
        /// <param name="timeout">Time in ms to wait</param>
        /// <returns></returns>
        public T Dequeue(int timeout)
        {
            return Dequeue(new TimeSpan(0, 0, 0, 0, timeout));
        }

        /// <summary>
        /// Dequeues the object at the front of the queue and waits indefinitely
        /// </summary>
        public T Dequeue()
        {
            return Dequeue(Timeout.Infinite);
        }

        /// <summary>
        /// Peeks at the object at the front of the queue
        /// </summary>
        public T Peek()
        {
            return _queue.First.Value;
        }

        /// <summary>
        /// Determines if the queue contains the supplied object
        /// </summary>
        public bool Contains(T obj)
        {
            return _queue.Contains(obj);
        }

        /// <summary>
        /// Waits until the queue has emptied
        /// </summary>
        public void WaitOnEmpty()
        {
            WaitOnEmpty(Timeout.Infinite);
        }

        /// <summary>
        /// Waits for the specified amount of time, or until the queue has emptied
        /// </summary>
        /// <param name="timeout">Time in ms to wait</param>
        public void WaitOnEmpty(int timeout)
        {
            lock (_starveRoot)
            {
                // We will block until count is 0.
                // We use Interlocked just to be sure we test for zero correctly as we
                // are not in the syncLock context.
                if (Count > 0)
                {
                    Monitor.Wait(_starveRoot, timeout);
                }
            }
        }

        /// <summary>
        /// Clears the queue, and places the queue in release mode.  Once there are no more
        /// waiting threads, release mode will stop.
        /// </summary>
        public void Clear()
        {
            lock (_syncLock)
            {
                _queue.Clear();
                _releasing = true;
                Monitor.PulseAll(_syncLock);
                lock (_starveRoot)
                {
                    Monitor.PulseAll(_starveRoot);
                }
            }
        }

        /// <summary>
        /// Waits for the specified amount of time, or until the queue has emptied then clears the queue
        /// </summary>
        public void ReleaseWaitingThreads()
        {
            lock (_syncLock)
            {
                while (Count != 0)
                {
                    WaitOnEmpty(100);
                }
                _releasing = true;

                Monitor.PulseAll(_syncLock);
            }

            lock (_starveRoot)
            {
                Monitor.PulseAll(_starveRoot);
            }
        }

        #endregion Public Methods
    }
}
