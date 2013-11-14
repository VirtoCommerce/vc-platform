using System;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using System.Windows;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    public partial class VirtualList<T> : IDisposable, IVirtualList, IList<VirtualListItem<T>>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public const int DefaultPageSize = 50;
        private static readonly NotifyCollectionChangedEventArgs _collectionReset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        private static readonly PropertyChangedEventArgs _totalItemsCountPropertyChanged = new PropertyChangedEventArgs("TotalItemsCount");

        IVirtualListLoader<T> _loader;
        int _version;
        int _pageSize;
        VirtualListItem<T>[] _list;
        QueuedBackgroundWorker<int> _pageRequests;
        readonly SynchronizationContext _synchronizationContext;

        public VirtualList(IVirtualListLoader<T> loader)
            : this(loader, DefaultPageSize, SynchronizationContext.Current)
        {
        }

        public VirtualList(IVirtualListLoader<T> loader, SynchronizationContext synchronizationContext)
            : this(loader, DefaultPageSize, synchronizationContext)
        {
        }

        public VirtualList(IVirtualListLoader<T> loader, int pageSize, SynchronizationContext synchronizationContext)
        {
            if (loader == null)
                throw new ArgumentNullException("loader");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize");

            _synchronizationContext = synchronizationContext;
            _pageRequests = new QueuedBackgroundWorker<int>(LoadPage, synchronizationContext);
            _pageRequests.StateChanged += new EventHandler(OnPageRequestsStateChanged);

            _version++;
            _loader = loader;
            _pageSize = pageSize;
            LoadAsync(0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _pageRequests.Clear();
        }

        public void Refresh()
        {
            ThrowIfDeferred();
            _list = null;
            SetCurrent(null, -1);
            LoadAsync(0);
        }

        public int TotalItemsCount
        {
            get
            {
                return Count;
            }
        }

        public QueuedBackgroundWorkerState LoadingState
        {
            get { return _pageRequests.State; }
        }

        public Exception LastLoadingError
        {
            get { return _pageRequests.LastError; }
        }

        void OnPageRequestsStateChanged(object sender, EventArgs e)
        {
            if (LoadingStateChanged != null && Application.Current != null)
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, (Action)delegate
                {
                    LoadingStateChanged(this, EventArgs.Empty);
                });
        }

        public event EventHandler LoadingStateChanged;

        public void RetryLoading()
        {
            if (LoadingState == QueuedBackgroundWorkerState.StoppedByError)
                _pageRequests.Retry();
        }

        private void PopulatePageData(int startIndex, IList<T> pageData, int overallCount)
        {
            bool flagRefresh = false;
            if (_list == null || _list.Length != overallCount)
            {
                _list = new VirtualListItem<T>[overallCount];
                flagRefresh = true;
            }
            for (int i = 0; i < pageData.Count; i++)
            {
                int index = startIndex + i;
                if (_list[index] == null)
                    _list[index] = new VirtualListItem<T>(this, index, pageData[i]);
                else
                    _list[index].Data = pageData[i];
            }
            if (flagRefresh)
            {
                if (this._synchronizationContext == null || SynchronizationContext.Current != null)
                    FireCollectionReset(null);
                else
                    _synchronizationContext.Send(FireCollectionReset, null);
            }
        }

        private void FireCollectionReset(object arg)
        {
            SetCurrent(null, -1);
            OnCollectionReset();
        }

        internal void Load(int index)
        {
            int startIndex = index - (index % _pageSize);
            LoadRange(startIndex, _pageSize);
        }

        private void LoadPage(int pageIndex)
        {
            int startIndex = pageIndex * _pageSize;
            LoadRange(startIndex, _pageSize);
        }

        private void LoadRange(int startIndex, int count)
        {
            int overallCount;
            IList<T> result = _loader.LoadRange(startIndex, count, SortDescriptions, out overallCount);
            PopulatePageData(startIndex, result, overallCount);
            OnPropertyChanged(_totalItemsCountPropertyChanged);
        }

        internal void LoadAsync(int index)
        {
            int pageIndex = index / _pageSize;
            _pageRequests.Add(pageIndex);
        }

        internal int Version
        {
            get { return _version; }
        }

        public bool Contains(VirtualListItem<T> item)
        {
            return IndexOf(item) != -1;
        }

        public int IndexOf(VirtualListItem<T> item)
        {
            return item == null || item.List != this ? -1 : item.Index;
        }

        public void CopyTo(VirtualListItem<T>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (arrayIndex >= array.Length)
                throw new ArgumentException("arrayIndex is greater or equal than the array length");
            if (arrayIndex + Count > array.Length)
                throw new ArgumentException("Number of elements in list is greater than available space");
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    Refresh();
                }
            }
        }

        public int Count
        {
            get { return _list == null ? 0 : _list.Length; }
        }

        public VirtualListItem<T> this[int index]
        {
            get
            {
                if (_list[index] == null)
                    _list[index] = new VirtualListItem<T>(this, index);
                return _list[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #region IList<VirtualListItem<T>> Members

        void IList<VirtualListItem<T>>.Insert(int index, VirtualListItem<T> item)
        {
            throw new NotSupportedException();
        }

        void IList<VirtualListItem<T>>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ICollection<VirtualListItem<T>> Members

        void ICollection<VirtualListItem<T>>.Add(VirtualListItem<T> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<VirtualListItem<T>>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<VirtualListItem<T>>.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<VirtualListItem<T>>.Remove(VirtualListItem<T> item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<VirtualListItem<T>> Members

        IEnumerator<VirtualListItem<T>> IEnumerable<VirtualListItem<T>>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        #endregion

        #region INotifyPropertyChanged Members

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        #endregion

        #region INotifyCollectionChanged Members

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                {
                    CollectionChanged(this, e);
                });
        }

        void OnCollectionReset()
        {
            OnCollectionChanged(_collectionReset);
        }


        event NotifyCollectionChangedEventHandler CollectionChanged;
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        #endregion
    }
}
