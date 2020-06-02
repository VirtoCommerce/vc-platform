using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    // http://sanjeev.dwivedi.net/?p=292
    // http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266983.aspx
    public class AsyncSemaphore
    {
        private readonly static Task s_completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> m_waiters = new Queue<TaskCompletionSource<bool>>();
        private int m_currentCount;

        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0) throw new ArgumentOutOfRangeException("initialCount");
            m_currentCount = initialCount;
        }

        public Task WaitAsync()
        {
            lock (m_waiters)
            {
                if (m_currentCount > 0)
                {
                    --m_currentCount;
                    return s_completed;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();
                    m_waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (m_waiters)
            {
                if (m_waiters.Count > 0)
                    toRelease = m_waiters.Dequeue();
                else
                    ++m_currentCount;
            }
            if (toRelease != null)
                toRelease.SetResult(true);
        }
    }

    // http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/10266988.aspx
    public class AsyncLock
    {
        private readonly string m_key;
        private readonly AsyncSemaphore m_semaphore;
        private readonly Task<Releaser> m_releaser;
        private static ConcurrentDictionary<string, AsyncLock> _lockMap = new ConcurrentDictionary<string, AsyncLock>();

        public AsyncLock(string key)
        {
            m_semaphore = new AsyncSemaphore(1);
            m_releaser = Task.FromResult(new Releaser(this, () => _lockMap.TryRemove(key, out var _)));
            m_key = key;
        }

        public static AsyncLock GetLockByKey(string key)
        {
            return _lockMap.GetOrAdd(key, (x) => new AsyncLock(key));
        }

        public Task<Releaser> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();
            return wait.IsCompleted ?
                m_releaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state, () => _lockMap.TryRemove(m_key, out var _)),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;
            private readonly Action m_postReleaseAction;

            internal Releaser(AsyncLock toRelease, Action postReleaseAction = null)
            {
                m_toRelease = toRelease;
                m_postReleaseAction = postReleaseAction;
            }

            public void Dispose()
            {
                if (m_toRelease != null)
                {
                    m_toRelease.m_semaphore.Release();
                    m_postReleaseAction?.Invoke();
                }
            }
        }
    }
}
