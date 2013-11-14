using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace  VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
	partial class VirtualList<T> : ICollectionView, ICollectionViewFactory
    {
        private static readonly PropertyChangedEventArgs _culturePropertyChanged = new PropertyChangedEventArgs("Culture");
        private static readonly PropertyChangedEventArgs _isCurrentBeforeFirstChanged = new PropertyChangedEventArgs("IsCurrentBeforeFirst");
        private static readonly PropertyChangedEventArgs _isCurrentAfterLastChanged = new PropertyChangedEventArgs("IsCurrentAfterLast");
        private static readonly PropertyChangedEventArgs _currentPositionChanged = new PropertyChangedEventArgs("CurrentPosition");
        private static readonly PropertyChangedEventArgs _currentItemChanged = new PropertyChangedEventArgs("CurrentItem");
        private int _deferRefreshCount;
        private bool _needsRefresh;
        private CultureInfo _cultureInfo;
        private int _currentPosition = -1;
        private VirtualListItem<T> _currentItem;
        private bool _isCurrentAfterLast = false;
        private bool _isCurrentBeforeFirst = true;
        private SortDescriptionCollection _sortDescriptionCollection;

        private class RefreshDeferrer : IDisposable
        {
            private VirtualList<T> _list;

            public RefreshDeferrer(VirtualList<T> list)
            {
                _list = list;
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_list != null)
                {
                    _list.EndDeferRefresh();
                    _list = null;
                }
            }

            #endregion
        }

        private bool IsRefreshDeferred
        {
            get { return _deferRefreshCount > 0; }
        }

        private void ThrowIfDeferred()
        {
            if (IsRefreshDeferred)
                throw new Exception("Can't do this while CollectionView refresh is deferred.");
        }

        private void RefreshOrDefer()
        {
            if (IsRefreshDeferred)
                _needsRefresh = true;
            else
                Refresh();
        }

        private void EndDeferRefresh()
        {
            if (0 == --_deferRefreshCount && _needsRefresh)
            {
                _needsRefresh = false;
                Refresh();
            }
        }

        private void OnCurrentChanged()
        {
            if (CurrentChanged != null)
                CurrentChanged(this, EventArgs.Empty);
        }

        private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshOrDefer();
        }

        private void SetCurrent(VirtualListItem<T> newItem, int newPosition)
        {
            bool isCurrentBeforeFirst = _isCurrentBeforeFirst;
            bool isCurrentAfterLast = _isCurrentAfterLast;
            VirtualListItem<T> currentItem = _currentItem;
            int currentPosition = _currentPosition;

            _isCurrentBeforeFirst = newPosition < 0;
            _isCurrentAfterLast = newPosition >= Count;
            _currentItem = newItem;
            _currentPosition = newPosition;

            if (currentItem != _currentItem)
                OnCurrentChanged();

            if (isCurrentBeforeFirst != _isCurrentBeforeFirst)
                OnPropertyChanged(_isCurrentBeforeFirstChanged);
            if (isCurrentAfterLast != _isCurrentAfterLast)
                OnPropertyChanged(_isCurrentAfterLastChanged);
            if (currentItem != _currentItem)
                OnPropertyChanged(_currentItemChanged);
            if (currentPosition != _currentPosition)
                OnPropertyChanged(_currentPositionChanged);
        }

        private bool OnCurrentChanging()
        {
            if (CurrentChanging == null)
                return true;
            else
            {
                CurrentChangingEventArgs e = new CurrentChangingEventArgs();
                CurrentChanging(this, e);
                return !e.Cancel;
            }
        }

        #region ICollectionView Members

        public bool CanFilter
        {
            get { return false; }
        }

		public bool CanGroup
        {
            get { return false; }
        }

        public bool CanSort
        {
			get { return _loader.CanSort; }
        }


		public bool Contains(object item)
        {
			return Contains(item as VirtualListItem<T>);
        }

        public CultureInfo Culture
        {
            get { return _cultureInfo; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_cultureInfo != value)
                {
                    _cultureInfo = value;
                    OnPropertyChanged(_culturePropertyChanged);
                }
            }
        }

        public event EventHandler CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;

		public object CurrentItem
        {
            get
            {
                ThrowIfDeferred();
                return _currentItem;
            }
        }


        public int CurrentPosition
        {
            get
            {
                ThrowIfDeferred();
                return _currentPosition;
            }
        }

        public IDisposable DeferRefresh()
        {
            ++_deferRefreshCount;
            return new RefreshDeferrer(this);
        }

		public Predicate<object> Filter
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

		public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return null; }
        }

		public ReadOnlyObservableCollection<object> Groups
        {
            get { return null; }
        }

		public bool IsCurrentAfterLast
        {
            get
            {
                ThrowIfDeferred();
                return _isCurrentAfterLast; 
            }
        }

		public bool IsCurrentBeforeFirst
        {
            get 
            {
                ThrowIfDeferred();
                return _isCurrentBeforeFirst; 
            }
        }

		public bool IsEmpty
        {
            get { return Count == 0; }
        }

		public bool MoveCurrentTo(object item)
        {
            ThrowIfDeferred();
            int position = IndexOf(item as VirtualListItem<T>);
            return this.MoveCurrentToPosition(position);
        }

		public bool MoveCurrentToFirst()
        {
            ThrowIfDeferred();
            return MoveCurrentToPosition(0);
        }

		public bool MoveCurrentToLast()
        {
            ThrowIfDeferred();
            return MoveCurrentToPosition(Count - 1);
        }

		public bool MoveCurrentToNext()
        {
            ThrowIfDeferred();
            int position = _currentPosition + 1;
            return position <= Count && MoveCurrentToPosition(position);
        }

		public bool MoveCurrentToPosition(int position)
        {
            ThrowIfDeferred();
            if (position < -1 || position > Count)
                throw new ArgumentOutOfRangeException("position");

            if (position != _currentPosition && OnCurrentChanging())
            {
                if (position == -1 || position == Count)
                    SetCurrent(null, position);
                else
                    SetCurrent(this[position], position);
            }
            return true;
          }

		public bool MoveCurrentToPrevious()
        {
            ThrowIfDeferred();
            int position = _currentPosition - 1;
            return position >= -1 && MoveCurrentToPosition(position);
        }

		public SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (!CanSort)
                    return null;

                if (_sortDescriptionCollection == null)
                {
                    _sortDescriptionCollection = new SortDescriptionCollection();
                    ((INotifyCollectionChanged)_sortDescriptionCollection).CollectionChanged += SortDescriptionsChanged;
                }
                return _sortDescriptionCollection;
            }
        }

		public IEnumerable SourceCollection
        {
            get { return this; }
        }

        #endregion

        #region ICollectionViewFactory Members

        public ICollectionView CreateView()
        {
            return this;
        }

        #endregion

    }
}
