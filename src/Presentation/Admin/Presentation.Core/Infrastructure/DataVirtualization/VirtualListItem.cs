using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace  VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    public sealed class VirtualListItem<T> : VirtualListItemBase, INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs s_isLoadedPropertyChanged = new PropertyChangedEventArgs("IsLoaded");
        private static readonly PropertyChangedEventArgs s_dataPropertyChanged = new PropertyChangedEventArgs("Data");

        VirtualList<T> _list;
        int _listVersion;
        int _index;
        bool _isLoaded;
        T _data;
        
        internal VirtualListItem(VirtualList<T> list, int index)
        {
            Debug.Assert(list != null);
            Debug.Assert(index >= 0 && index < list.Count);
            _list = list;
            _listVersion = list.Version;
            _index = index;
        }

        internal VirtualListItem(VirtualList<T> list, int index, T data)
            : this(list, index)
        {
            Data = data;
        }

        public VirtualList<T> List
        {
            get { return (_list.Version == _listVersion) ? _list : null; }
        }

        public int Index
        {
            get { return _index; }
        }

        public sealed override bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public new T Data
        {
            get { return _data; }
            internal set
            {
                _data = value;
                _isLoaded = true;
                OnPropertyChanged(s_isLoadedPropertyChanged);
                OnPropertyChanged(s_dataPropertyChanged);
            }
        }

        internal override object GetData()
        {
            return _data;
        }

        public sealed override void Load()
        {
            if (IsLoaded)
                return;
            List.Load(Index);
        }

        public override void LoadAsync()
        {
            if (IsLoaded)
                return;
            List.LoadAsync(Index);
        }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(PropertyChangedEventArgs e)
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
    }

}
