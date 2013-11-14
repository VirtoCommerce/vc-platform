using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class DataGridScrollBehavior
    {
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        [AttachedPropertyBrowsableForType(typeof(ListView))]
        [AttachedPropertyBrowsableForType(typeof(ListBox))]
        public static bool GetAutoAcrollIntoView(Selector selector)
        {
            return (bool)selector.GetValue(AutoScrollIntoViewProperty);
        }

        public static void SetAutoScrollIntoView(Selector selector, bool value)
        {
            selector.SetValue(AutoScrollIntoViewProperty, value);
        }


        public static readonly DependencyProperty AutoScrollIntoViewProperty =
            DependencyProperty.RegisterAttached("AutoScrollIntoView", typeof (bool), typeof (DataGridScrollBehavior),
                                                new UIPropertyMetadata(false, OnAutoScrollIntoViewSelectionChanged));


        static void OnAutoScrollIntoViewSelectionChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            Selector selector = depObj as Selector;
            if (selector == null)
                return;

            if ((bool) e.NewValue)
            {
                selector.SelectionChanged += OnSelectorSelectionChanged;
                //if (selector is ListView)
                //{
                //    ((INotifyCollectionChanged) selector.Items).CollectionChanged += (s, args) =>
                //        {
                //            if (args.Action == NotifyCollectionChangedAction.Add)
                //            {
                //                ListView listView = selector as ListView;
                //                if (listView != null)
                //                {
                //                    listView.SelectedItem = args.NewItems[0];
                //                    listView.ScrollIntoView(args.NewItems[0]);
                //                }
                //            }
                //        };

                //}
            }
            else
            {
                selector.SelectionChanged -= OnSelectorSelectionChanged;
            }

        }

       

        private static void OnSelectorSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            Selector selector = e.OriginalSource as Selector;


            if (selector is DataGrid)
            {
                if (selector != null && selector.SelectedItem != null)
                {
                    selector.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                                    new DispatcherOperationCallback(ScrollItemIntoView_DataGrid),
                                                    selector);
                }
            }
            else if (selector is ListView)
            {
                if (selector != null && selector.SelectedItem != null)
                {
                    selector.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                                    new DispatcherOperationCallback(ScrollItemIntoView_ListView),
                                                    selector);
                }
            }
            else if (selector is ListBox)
            {
                if (selector != null && selector.SelectedItem != null)
                {
                    selector.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                                    new DispatcherOperationCallback(ScrollItemIntoView_ListBox),
                                                    selector);
                }
            }

        }

        static object ScrollItemIntoView_DataGrid(object sender)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            }
            return null;
        }

        static object ScrollItemIntoView_ListBox(object sender)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
            return null;
        }

        static object ScrollItemIntoView_ListView(object sender)
        {
            ListView listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                listView.ScrollIntoView(listView.SelectedItem);
            }
            return null;
        }
    }
}
