using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Control for showing available and current items, in a ListBox. User can add items
    /// from the available list to the current list, and remove items from the current
    /// list. In the available list only items that are not in the current list are shown.
    /// Important: User should make sure that DataContext is set properly. 
    /// </summary>
    [TemplatePart(Name = "PART_CurrentListBox", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_AvailableListBox", Type = typeof(ListBox))]
    public partial class MultiSelectControl : ContentControl
    {
        #region Properties

        /// <summary>
        /// The default colleciton view of the List used as the Items source for the 
        /// Available Items ListBox.
        /// </summary>
        public ICollectionView AvailableItemsCollectionView { get; private set; }

        /// <summary>
        /// A reference to the Available Items ListBox.
        /// </summary>
        public ListBox AvailabeListBox { get; private set; }

        /// <summary>
        /// A reference to the Current Items ListBox.
        /// </summary>
        public ListBox CurrentListBox { get; private set; }

        #endregion

        #region Dependency Properties

        public string TextSearchTextPath
        {
            get { return (string)GetValue(TextSearchTextPathProperty); }
            set { SetValue(TextSearchTextPathProperty, value); }
        }
        public static readonly DependencyProperty TextSearchTextPathProperty =
            DependencyProperty.Register("TextSearchTextPath", typeof(string), typeof(MultiSelectControl));

        public Visibility UpDownButtonsVisible
        {
            get { return (Visibility)GetValue(UpDownButtonsVisibleProperty); }
            set { SetValue(UpDownButtonsVisibleProperty, value); }
        }
        public static readonly DependencyProperty UpDownButtonsVisibleProperty =
            DependencyProperty.Register("UpDownButtonsVisible", typeof(Visibility), typeof(MultiSelectControl), new UIPropertyMetadata(Visibility.Collapsed));

        public DataTemplate ObjectsTemplate2
        {
            get
            {
                var retVal = (DataTemplate)GetValue(ObjectsTemplate2Property);
                if (retVal == null)
                {
                    retVal = ObjectsTemplate;
                }
                return retVal;
            }
            set
            {
                SetValue(ObjectsTemplate2Property, value);
            }
        }
        public static readonly DependencyProperty ObjectsTemplate2Property =
            DependencyProperty.Register(
                "ObjectsTemplate2", typeof(DataTemplate),
                typeof(MultiSelectControl),
                new FrameworkPropertyMetadata()
        );

        /// <summary>
        /// A DataTemplate used to display the available and current items in the 
        /// listboxes.
        /// </summary>
        public DataTemplate ObjectsTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ObjectsTemplateProperty);
            }
            set
            {
                SetValue(ObjectsTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty ObjectsTemplateProperty =
            DependencyProperty.Register(
                "ObjectsTemplate", typeof(DataTemplate),
                typeof(MultiSelectControl),
                new FrameworkPropertyMetadata()
        );


        /// <summary>
        /// The items used as an ItemsSource for the available ListBox. It is recommended
        /// to use an ObservableCollection of an [Object], where the [Object] implements
        /// INotifyPropertyChanged and overrides the Equals method.
        /// </summary>
        public object AvailableItems
        {
            get
            {
                return (object)GetValue(AvailableItemsProperty);
            }
            set
            {
                SetValue(AvailableItemsProperty, value);
            }
        }
        public static readonly DependencyProperty AvailableItemsProperty =
            DependencyProperty.Register(
                "AvailableItems", typeof(object),
                typeof(MultiSelectControl),
                new PropertyMetadata(AvailableItemsPropertyChangedCallback));

        /// The items used as an ItemsSource for the current ListBox. It is recommended
        /// to use an ObservableCollection of an [Object], where the [Object] implements
        /// INotifyPropertyChanged and Equals.
        public object CurrentItems
        {
            get
            {
                return (object)GetValue(CurrentItemsProperty);
            }
            set
            {
                SetValue(CurrentItemsProperty, value);
            }
        }
        public static readonly DependencyProperty CurrentItemsProperty =
            DependencyProperty.Register(
                "CurrentItems", typeof(object),
                typeof(MultiSelectControl),
                new PropertyMetadata(CurrentItemsPropertyChangedCallback));


        /// <summary>
        /// The string title to be displayed above the Available Items ListBox.
        /// </summary>
        public string AvailableTitle
        {
            get
            {
                return (string)GetValue(AvailableTitleProperty);
            }
            set
            {
                SetValue(AvailableTitleProperty, value);
            }
        }
        public static readonly DependencyProperty AvailableTitleProperty =
            DependencyProperty.Register(
                "AvailableTitle", typeof(string),
                typeof(MultiSelectControl),
                new FrameworkPropertyMetadata("Available")
        );

        /// <summary>
        /// The string title to be displayed above the Current Items ListBox.
        /// </summary>
        public string CurrentTitle
        {
            get
            {
                return (string)GetValue(CurrentTitleProperty);
            }
            set
            {
                SetValue(CurrentTitleProperty, value);
            }
        }
        public static readonly DependencyProperty CurrentTitleProperty =
            DependencyProperty.Register(
                "CurrentTitle", typeof(string),
                typeof(MultiSelectControl),
                new FrameworkPropertyMetadata("Current")
        );

        #endregion

        #region Constructors

        /// <summary>
        /// Default CTOR. Initializes the control, and add loaded event handlers.
        /// </summary>
        public MultiSelectControl()
        {
            InitializeComponent();
        }

        static void AvailableItemsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                ((MultiSelectControl)d).SetupAvailableItems();
        }

        /// <summary>
        /// setup Available Items list to filter by Current Items list.
        /// </summary>
        private void SetupAvailableItems()
        {
            AvailableItemsCollectionView = CollectionViewSource.GetDefaultView(AvailableItems);
            if (AvailableItemsCollectionView.Filter == null)
                AvailableItemsCollectionView.Filter = new Predicate<object>(FilterOutCurrentItems);
            else
                AvailableItemsCollectionView.Refresh();
        }

        static void CurrentItemsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                ((MultiSelectControl)d).SetupCurrentItems();
        }

        private void SetupCurrentItems()
        {
            var currentCollectionView = CollectionViewSource.GetDefaultView(CurrentItems);
            UpDownButtonsVisible = currentCollectionView.SortDescriptions.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            if (AvailableItemsCollectionView != null)
                AvailableItemsCollectionView.Refresh();
        }

        /// <summary>
        /// Allow for setting the style.
        /// </summary>
        static MultiSelectControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MultiSelectControl),
                new FrameworkPropertyMetadata(typeof(MultiSelectControl))
            );
        }
        #endregion

        #region methods

        /// <summary>
        /// On initialize verify that the template is applied to the control.
        /// </summary>
        //void MultiSelectControl_Initialized(object sender, EventArgs e)
        //{
        //    ApplyTemplate();
        //}

        /// <summary>
        /// Occurs when the template is applied. Sets the different references to the 
        /// template parts
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // ControlTemplate ct = Template;

            ListBox available = Template.FindName("PART_AvailableListBox", this) as ListBox;
            if (null != available)
            {
                AvailabeListBox = available;
            }

            ListBox current = Template.FindName("PART_CurrentListBox", this) as ListBox;
            if (null != current)
            {
                CurrentListBox = current;
                if (current.ItemTemplate == null)
                {
                    // current.ItemTemplate = AvailabeListBox.ItemTemplate;
                    current.ItemTemplate = ObjectsTemplate2;
                }
            }

            //AvailableItemsCollectionView = CollectionViewSource.GetDefaultView(AvailableItems);
            //AvailableItemsCollectionView.Filter = new Predicate<object>(FilterOutTextAndCurrentItems);
        }

        /// <summary>
        /// Filter out an item, if it exists in current Items collection. Used on the 
        /// Available Items list.
        /// Notice the given object must implement the Equals method in a meaningful way.
        /// </summary>
        /// <param name="item">
        /// The item to filter
        /// </param>
        /// <returns>
        /// True if the item passes the filter, false otherwise.
        /// </returns>
        public bool FilterOutCurrentItems(object item)
        {
            ICollection currentItems = CurrentItems as ICollection;
            if (null != currentItems)
            {
                //check if object is contained in current items
                foreach (object obj in currentItems)
                {
                    if (obj.Equals(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            //current Items is null
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove selected items from current list. 
        /// </summary>
        void LeftArrow_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is IMultiSelectControlCommands)
            {
                var externalCommands = (IMultiSelectControlCommands)this.DataContext;
                if (CurrentListBox.SelectedItems.Count > 0)
                {
                    externalCommands.UnSelectItem(CurrentListBox.SelectedItems[0]);
                }
            }
            else
            {
                //A copy is used, because the collection is changed in the iteration
                IList currentSelectedItems =
                    new List<object>((IEnumerable<object>)CurrentListBox.SelectedItems);
                IList currentListItems = CurrentItems as IList;
                if (null != currentListItems)
                {
                    foreach (object obj in currentSelectedItems)
                    {
                        currentListItems.Remove(obj);
                    }
                }
            }
            //updates the available collection
            AvailableItemsCollectionView.Refresh();
        }

        /// <summary>
        /// add selected available items to Current Items list.
        /// </summary>
        void RightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is IMultiSelectControlCommands)
            {
                var externalCommands = (IMultiSelectControlCommands)this.DataContext;
                if (AvailabeListBox.SelectedItems.Count > 0)
                {
                    externalCommands.SelectItem(AvailabeListBox.SelectedItems[0]);
                }
            }
            else
            {
                //A copy is used, because the collection is changed in the iteration
                IList selectedItems =
                    new List<object>((IEnumerable<object>)AvailabeListBox.SelectedItems);
                IList currentListItems = CurrentItems as IList;
                if (null != currentListItems)
                {
                    foreach (object obj in selectedItems)
                    {
                        currentListItems.Add(obj);
                    }
                }
            }

            //updates the available collection
            AvailableItemsCollectionView.Refresh();
        }

        /// <summary>
        /// Remove all items from current list. 
        /// </summary>
        void DoubleLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            IList currentListItems = CurrentItems as IList;

            if (this.DataContext is IMultiSelectControlCommands)
            {
                var externalCommands = (IMultiSelectControlCommands)this.DataContext;
                externalCommands.UnSelectAllItems(currentListItems);
            }
            else
            {
                if (null != currentListItems)
                {
                    currentListItems.Clear();
                }
            }

            //updates the available collection
            AvailableItemsCollectionView.Refresh();
        }

        /// <summary>
        /// Add all Available Items to Current Items list.
        /// </summary>
        void DoubleRightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is IMultiSelectControlCommands)
            {
                var externalCommands = (IMultiSelectControlCommands)this.DataContext;
                externalCommands.SelectAllItems(AvailableItemsCollectionView);
            }
            else
            {
                IList currentListItems = CurrentItems as IList;
                if (null != currentListItems)
                {
                    foreach (object obj in AvailableItemsCollectionView)
                    {
                        currentListItems.Add(obj);
                    }
                }
            }

            AvailableItemsCollectionView.Refresh();
        }

        /// <summary>
        /// On double click on listbox item, move selected item from available to current.
        /// </summary>
        private void AvailableListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RightArrow_Click(sender, e);
            e.Handled = true;
        }

        /// <summary>
        /// On double click on listbox item, move selected it from current to available.
        /// </summary>
        private void CurrentListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LeftArrow_Click(sender, e);
            e.Handled = true;
        }

        void UpArrow_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentListBox.SelectedIndex > 0)
            {
                SwitchSelectedItemPosition(-1);
            }
        }

        void DownArrow_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentListBox.SelectedIndex < CurrentListBox.Items.Count - 1)
            {
                SwitchSelectedItemPosition(1);
            }
        }

        /// <summary>
        /// Switches current SelectedItem with another item
        /// </summary>
        /// <param name="otherItemIndexDelta">another item's index relative to current SelectedItem index</param>
        private void SwitchSelectedItemPosition(int otherItemIndexDelta)
        {
            var currentCollectionView = CollectionViewSource.GetDefaultView(CurrentItems);
            if (currentCollectionView.SortDescriptions.Count > 0)
            {
                var switchingItem = CurrentListBox.Items[CurrentListBox.SelectedIndex + otherItemIndexDelta];
                var selectedItem = CurrentListBox.SelectedItem;

                if (switchingItem != null && selectedItem != null)
                {
                    var propertyName = currentCollectionView.SortDescriptions[0].PropertyName;
                    var sortProperty = selectedItem.GetType().GetProperty(propertyName);

                    var selectedItemValue = sortProperty.GetValue(selectedItem, null);
                    var switchingItemValue = sortProperty.GetValue(switchingItem, null);

                    sortProperty.SetValue(selectedItem, switchingItemValue, null);
                    sortProperty.SetValue(switchingItem, selectedItemValue, null);

                    currentCollectionView.Refresh();
                }
            }
        }
        #endregion
    }

    public interface IMultiSelectControlCommands
    {
        void SelectItem(object selectedObj);
        void SelectAllItems(ICollectionView availableItemsCollectionView);

        void UnSelectItem(object selectedObj);
        void UnSelectAllItems(IList currentListItems);
    }

}
