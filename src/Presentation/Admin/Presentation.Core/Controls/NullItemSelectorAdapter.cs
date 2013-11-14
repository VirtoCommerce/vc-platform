using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Adapts a <see cref="Selector"/> control to include an item representing null.
    /// This element is a <see cref="ContentControl"/> whose <see cref="ContentControl.Content"/>
    /// should be a Selector, such as a <see cref="ComboBox"/>, <see cref="ListBox"/>,
    /// or <see cref="ListView"/>.
    /// </summary>
    /// <remarks>
    /// In XAML, place this element immediately outside the target Selector, and set the
    /// <see cref="ItemsSource"/> property instead of the Selector's ItemsSource.
    /// </remarks>
    /// <example>
    /// <code>
    /// <local:NullItemSelectorAdapter ItemsSource=&quot;{Binding CustomerList}&quot;>
    ///     <ComboBox .../>
    /// </local:NullItemSelectorAdapter>
    /// </code>
    /// </example>
    [ContentProperty("Selector")]
    public class NullItemSelectorAdapter : ContentControl
    {
        //ICollectionView _collectionView;
        ///// <summary>
        ///// Gets or sets the collection view associated with the internal <see cref="CompositeCollection"/>
        ///// that combines the null-representing item and the <see cref="ItemsSource"/>.
        ///// </summary>
        //protected ICollectionView CollectionView
        //{
        //    get { return _collectionView; }
        //    set { _collectionView = value; }
        //}

        /// <summary>
        /// Identifies the <see cref="Selector"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectorProperty = DependencyProperty.Register(
            "Selector", typeof(Selector), typeof(NullItemSelectorAdapter),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(Selector_Changed)));
        static void Selector_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NullItemSelectorAdapter adapter = (NullItemSelectorAdapter)sender;
            adapter.Content = e.NewValue;
            Selector selector = (Selector)e.OldValue;
            if (selector != null) selector.SelectionChanged -= adapter.Selector_SelectionChanged;
            selector = (Selector)e.NewValue;
            if (selector != null)
            {
                selector.IsSynchronizedWithCurrentItem = true;
                selector.SelectionChanged += adapter.Selector_SelectionChanged;
            }
            adapter.Adapt();
        }

        /// <summary>
        /// Gets or sets the Selector control.
        /// </summary>
        public Selector Selector
        {
            get { return (Selector)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(ICollection), typeof(NullItemSelectorAdapter),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsSource_Changed)));
        static void ItemsSource_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NullItemSelectorAdapter adapter = (NullItemSelectorAdapter)sender;
            adapter.Adapt();
        }

        /// <summary>
        /// Gets or sets the data items.
        /// </summary>
        public ICollection ItemsSource
        {
            get { return (ICollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public IList NullItems
        {
            get { return (IList)GetValue(NullItemsProperty); }
            set { SetValue(NullItemsProperty, value); }
        }
        public static readonly DependencyProperty NullItemsProperty =
            DependencyProperty.Register("NullItems", typeof(IList), typeof(NullItemSelectorAdapter), new UIPropertyMetadata(null));

        /// <summary>
        /// null items placing location. true - beginning, otherwise - end. Default is false.
        /// </summary>
        public bool IsAddingNullItemsFirst
        {
            get { return (bool)GetValue(IsAddingNullItemsFirstProperty); }
            set { SetValue(IsAddingNullItemsFirstProperty, value); }
        }
        public static readonly DependencyProperty IsAddingNullItemsFirstProperty =
            DependencyProperty.Register("IsAddingNullItemsFirst", typeof(bool), typeof(NullItemSelectorAdapter), new UIPropertyMetadata(false));

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public NullItemSelectorAdapter()
        {
            IsTabStop = false;
        }

        /// <summary>
        /// Updates the Selector control's <see cref="ItemsControl.ItemsSource"/> to include the
        /// <see cref="NullItem"/> along with the objects in <see cref="ItemsSource"/>.
        /// </summary>
        protected void Adapt()
        {
            //if (CollectionView != null)
            //{
            //    CollectionView.CurrentChanged -= CollectionView_CurrentChanged;
            //    CollectionView = null;
            //}

            if (Selector != null)
            {
                //if (ItemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                //{
                //    var col = ItemsSource as System.Collections.Specialized.INotifyCollectionChanged;
                //    col.CollectionChanged -= ItemsSource_CollectionChanged;
                //    col.CollectionChanged += ItemsSource_CollectionChanged;
                //}

                CompositeCollection comp = new CompositeCollection();
                if (ItemsSource != null)
                {
                    comp.Add(new CollectionContainer { Collection = ItemsSource });
                }

                //comp.Add(new CollectionContainer { Collection = new string[] { "(None)" } });
                if (NullItems != null)
                {
                    foreach (var item in NullItems)
                    {
                        if (IsAddingNullItemsFirst)
                            comp.Insert(0, item);
                        else
                            comp.Add(item);
                    }
                }

                //CollectionView = CollectionViewSource.GetDefaultView(comp);
                //if (CollectionView != null) CollectionView.CurrentChanged += CollectionView_CurrentChanged;

                Selector.ItemsSource = comp;
            }
        }

        //void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    var awd = sender;
        //}

        bool _isChangingSelection;
        /// <summary>
        /// Triggers binding sources to be updated if the <see cref="NullItem"/> is selected.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event data</param>
        protected void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  Selector.SelectedItem == NullItem || 

            //if ((NullItems != null && NullItems.Contains(Selector.SelectedItem)))
            //{
            //    if (!_isChangingSelection)
            //    {
            //        _isChangingSelection = true;
            //        try
            //        {
            //            // Selecting the null item doesn't trigger an update to sources bound to properties
            //            // like SelectedItem, so move selection away and then back to force this.
            //            int selectedIndex = Selector.SelectedIndex;
            //            Selector.SelectedIndex = -1;
            //            Selector.SelectedIndex = selectedIndex;
            //        }
            //        finally
            //        {
            //            _isChangingSelection = false;
            //        }
            //    }
            //}

            // added because when selector value is allready null, the CollectionView_CurrentChanged doesn't get called
            if (!_isChangingSelection && Selector != null && Selector.SelectedItem == null)
            {
                _isChangingSelection = true;
                try
                {
                    if (NullItems != null && NullItems.Count > 0)
                        Selector.SelectedItem = NullItems[0];
                    else
                        Selector.SelectedIndex = 0;
                }
                finally
                {
                    _isChangingSelection = false;
                }
            }
        }

        /// <summary>
        /// Selects the <see cref="NullItem"/> if the source collection's current item moved to null.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event data</param>
        void CollectionView_CurrentChanged(object sender, EventArgs e)
        {
            //if (!_isChangingSelection && Selector != null && ((ICollectionView)sender).CurrentItem == null && Selector.Items.Count != 0)
            //{
            //    _isChangingSelection = true;
            //    try
            //    {
            //        if (NullItems != null && NullItems.Count > 0)
            //            Selector.SelectedItem = NullItems[0];
            //        else
            //            Selector.SelectedIndex = 0;
            //    }
            //    finally
            //    {
            //        _isChangingSelection = false;
            //    }
            //}
        }
    }
}
