using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;

namespace  VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    public sealed class VirtualListLoadingIndicator : Control
    {
        #region IsAttached attached property

        private sealed class ItemsControlManager : IDisposable
        {
            ItemsControl _itemsControl;
            IVirtualList _list;
            QueuedBackgroundWorkerState _loadingState = QueuedBackgroundWorkerState.Standby;

            public ItemsControlManager(ItemsControl itemsControl)
            {
                Debug.Assert(itemsControl != null);
                _itemsControl = itemsControl;
                SetState(itemsControl.ItemsSource as IVirtualList);
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
                dpd.AddValueChanged(itemsControl, OnItemsSourceChanged);
            }

            public void Dispose()
            {
                if (LoadingIndicator != null)
                    AdornerManager.SetAdorner(_itemsControl, null);

                if (_itemsControl != null)
                {
                    DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
                    dpd.RemoveValueChanged(_itemsControl, OnItemsSourceChanged);
                    _itemsControl = null;
                }
                if (_list != null)
                {
                    _list.LoadingStateChanged -= OnLoadingStateChanged;
                    _list = null;
                }
                _loadingState = QueuedBackgroundWorkerState.Standby;
                GC.SuppressFinalize(this);
            }

            void OnItemsSourceChanged(object sender, EventArgs e)
            {
                SetState(_itemsControl.ItemsSource as IVirtualList);
            }

            private IVirtualList List
            {
                get { return _list; }
                set
                {
                    if (_list == value)
                        return;

                    IVirtualList oldValue = _list;
                    _list = value;

                    if (oldValue != null)
                        oldValue.LoadingStateChanged -= OnLoadingStateChanged;
                    if (value != null)
                        value.LoadingStateChanged += OnLoadingStateChanged;
                }
            }

            private void OnLoadingStateChanged(object sender, EventArgs e)
            {
                IVirtualList list = (IVirtualList)sender;
                SetState(list.LoadingState);
            }

            private QueuedBackgroundWorkerState LoadingState
            {
                get { return _loadingState; }
                set { _loadingState = value; }
            }

            private void SetState(IVirtualList list)
            {
                SetState(list, list == null ? QueuedBackgroundWorkerState.Standby : list.LoadingState);
            }

            private void SetState(QueuedBackgroundWorkerState loadingState)
            {
                SetState(_list, loadingState);
            }

            private void SetState(IVirtualList list, QueuedBackgroundWorkerState loadingState)
            {
                if (list == List && loadingState == LoadingState)
                    return;

                List = list;
                LoadingState = loadingState;
                VirtualListLoadingIndicator loadingIndicator = LoadingIndicator;
                bool oldIsVisible = loadingIndicator != null;
                bool newIsVisible = LoadingState != QueuedBackgroundWorkerState.Standby;
                if (oldIsVisible == newIsVisible)
                {
                    if (newIsVisible)
                        loadingIndicator.SetState(List, LoadingState);
                }
                else if (newIsVisible)
                {
                    AdornerManager.SetAdorner(_itemsControl, new VirtualListLoadingIndicator(List, LoadingState));
                }
                else
                {
                    AdornerManager.SetAdorner(_itemsControl, null);
                }
            }

            private VirtualListLoadingIndicator LoadingIndicator
            {
                get { return AdornerManager.GetAdorner(_itemsControl) as VirtualListLoadingIndicator; }
            }
        }

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(VirtualListLoadingIndicator),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsAttachedChanged)));
        private static readonly DependencyProperty ItemsControlManagerProperty = DependencyProperty.RegisterAttached("ItemsControlManager", typeof(ItemsControlManager), typeof(VirtualListLoadingIndicator),
            new FrameworkPropertyMetadata(null));

        public static bool GetIsAttached(DependencyObject d)
        {
            return (bool)d.GetValue(IsAttachedProperty);
        }

        public static void SetIsAttached(DependencyObject d, bool value)
        {
            d.SetValue(IsAttachedProperty, value);
        }

        private static ItemsControlManager GetItemsControlManager(ItemsControl itemsControl)
        {
            return (ItemsControlManager)itemsControl.GetValue(ItemsControlManagerProperty);
        }

        private static void SetItemsControlManager(ItemsControl itemsControl, ItemsControlManager value)
        {
            itemsControl.SetValue(ItemsControlManagerProperty, value);
        }

        private static void ClearItemsControlManager(ItemsControl itemsControl)
        {
            itemsControl.ClearValue(ItemsControlManagerProperty);
        }

        static void OnIsAttachedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl itemsControl = d as ItemsControl;
            if (itemsControl == null)
                return;
            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;
            if (oldValue == newValue)
                return;

            if (newValue)
            {
                SetItemsControlManager(itemsControl, new ItemsControlManager(itemsControl));
            }
            else
            {
                var itemsControlManager = GetItemsControlManager(itemsControl);
                itemsControlManager.Dispose();
                ClearItemsControlManager(itemsControl);
            }
        }

        #endregion

        public static readonly RoutedCommand RetryCommand = new RoutedCommand("Retry", typeof(VirtualListLoadingIndicator));

        static VirtualListLoadingIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualListLoadingIndicator), new FrameworkPropertyMetadata(typeof(VirtualListLoadingIndicator)));
            CommandBinding retryCommandBinding = new CommandBinding(RetryCommand,
                new ExecutedRoutedEventHandler(OnRetryCommandExecuted),
                new CanExecuteRoutedEventHandler(CanExecuteRetryCommand));
            CommandManager.RegisterClassCommandBinding(typeof(VirtualListLoadingIndicator), retryCommandBinding);
        }

        private static void OnRetryCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((VirtualListLoadingIndicator)sender).RetryLoading();
        }

        private static void CanExecuteRetryCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            VirtualListLoadingIndicator control = (VirtualListLoadingIndicator)sender;
            e.CanExecute = control.HasError;
        }

        private IVirtualList _list;
        private QueuedBackgroundWorkerState _loadingState = QueuedBackgroundWorkerState.Standby;
        private static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterReadOnly("HasError", typeof(bool), typeof(VirtualListLoadingIndicator),
            new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;
        private static readonly DependencyPropertyKey ErrorMessagePropertyKey = DependencyProperty.RegisterReadOnly("ErrorMessage", typeof(string), typeof(VirtualListLoadingIndicator),
            new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ErrorMessageProperty = ErrorMessagePropertyKey.DependencyProperty;

        private VirtualListLoadingIndicator(IVirtualList list, QueuedBackgroundWorkerState loadingState)
        {
            SetState(list, loadingState);
        }

        void SetState(IVirtualList list, QueuedBackgroundWorkerState loadingState)
        {
            if (_list == list && _loadingState == loadingState)
                return;

            _list = list;
            _loadingState = loadingState;
            HasError = _loadingState == QueuedBackgroundWorkerState.StoppedByError;
            ErrorMessage = HasError ? _list.LastLoadingError.ToString() : null;
        }

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            private set { SetValue(HasErrorPropertyKey, value); }
        }

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            private set { SetValue(ErrorMessagePropertyKey, value); }
        }

        public void RetryLoading()
        {
            if (HasError)
                _list.RetryLoading();
        }
    }
}
