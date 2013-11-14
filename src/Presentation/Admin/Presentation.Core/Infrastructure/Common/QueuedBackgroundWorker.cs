using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public enum QueuedBackgroundWorkerState
    {
        Standby,
        Processing,
        StoppedByError,
    }

    /// <summary>Executes queued operations on a separate thread.</summary>
    /// <typeparam name="T">Type of work item argument provided to DoWork callback.</typeparam>
    /// <remarks>This class is thread safe.</remarks>
    public sealed class QueuedBackgroundWorker<T> : IDisposable
    {
        readonly SynchronizationContext _synchronizationContext;
        QueuedBackgroundWorkerState _currentState = QueuedBackgroundWorkerState.Standby;
        AutoResetEvent _processingWaitSignal;
        bool _flagClear;
        Queue<T> _queue = new Queue<T>();
        Action<T> _doWorkCallback;
        Exception _lastError;
        private object _lockObject = new object();

        public QueuedBackgroundWorker(Action<T> doWorkCallback)
            : this(doWorkCallback, SynchronizationContext.Current)
        {
        }

        public QueuedBackgroundWorker(Action<T> doWorkCallback, SynchronizationContext synchronizationContext)
        {
            if (doWorkCallback == null)
                throw new ArgumentNullException("doWorkCallback");

            _doWorkCallback = doWorkCallback;
            _synchronizationContext = synchronizationContext;
        }

        public void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public QueuedBackgroundWorkerState State
        {
            get { return _currentState; }
            set
            {
                if (_currentState == value)
                    return;

                _currentState = value;
                if (value == QueuedBackgroundWorkerState.Processing)
                {
                    _processingWaitSignal = new AutoResetEvent(false);
                    _flagClear = false;
                    _lastError = null;
					Task.Factory.StartNew(() => { Process(null); });
                    //ThreadPool.QueueUserWorkItem(Process);
                }
                else
                {
                    if (_processingWaitSignal != null)
                    {
                        _processingWaitSignal.Set();
                        _processingWaitSignal = null;
                    }
                }
                OnStateChanged();
            }
        }

        private void OnStateChanged()
        {
            if (this._synchronizationContext == null || SynchronizationContext.Current != null)
                RaiseStateChangedEvent(null);
            else
                _synchronizationContext.Send(RaiseStateChangedEvent, null);
        }

        void RaiseStateChangedEvent(object args)
        {
            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);
        }

        public event EventHandler StateChanged;

        public Exception LastError
        {
            get { return _lastError; }
        }

        public void Add(T workItem)
        {
            //lock (_lockObject)
            //{
                if (_queue.Contains(workItem))
                    return;
                _queue.Enqueue(workItem);
                //if (State == QueuedBackgroundWorkerState.Standby)
                State = QueuedBackgroundWorkerState.Processing;
            //}
        }

        private void Process(object arg)
        {
            while (true)
            {
                T workItem;
                lock (_lockObject)
                {
                    workItem = _queue.Peek();
                }
                try
                {
                    _doWorkCallback(workItem);
                }
                catch (Exception ex)
                {
                    lock (_lockObject)
                    {
                        _lastError = ex;
                        State = QueuedBackgroundWorkerState.StoppedByError;
                    }
                    return;
                }

                lock (_lockObject)
                {
                    if (_flagClear)
                        _queue.Clear();
                    else
                        _queue.Dequeue();

                    if (_queue.Count == 0)
                    {
                        State = QueuedBackgroundWorkerState.Standby;
                        return;
                    }
                }
            }
        }

        public void Clear()
        {
            AutoResetEvent waitSignal = null;
            lock (_lockObject)
            {
                if (State == QueuedBackgroundWorkerState.Processing)
                {
                    _flagClear = true;
                    waitSignal = _processingWaitSignal;
                }
                else
                {
                    _queue.Clear();
                    State = QueuedBackgroundWorkerState.Standby;
                }
            }

            // Wait for the completion of currently processing work item
            if (waitSignal != null)
                waitSignal.WaitOne();
        }

        public void Retry()
        {
            lock (_lockObject)
            {
                if (State == QueuedBackgroundWorkerState.StoppedByError)
                    State = QueuedBackgroundWorkerState.Processing;
            }
        }
    }
}
