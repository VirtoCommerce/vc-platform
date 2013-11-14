using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using CustomerModels = VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls
{
    //public static class TextExtensions
    //{
    //    public static int IndexOf<TSource>(this IEnumerable<TSource> source,
    //                                       Func<TSource, bool> predicate)
    //    {
    //        int i = 0;

    //        foreach (TSource element in source)
    //        {
    //            if (predicate(element))
    //                return i;

    //            i++;
    //        }

    //        return -1;
    //    }
    //}


    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    /// 
    public partial class TagControl : UserControl
    {
        public TagControl()
        {
            InitializeComponent();

            IsTextForSearchEmpty = true;
            IsControlInitialized = false;


            SpacePressedCommand = new DelegateCommand<string>(SpacePressed);
            DeleteItemCommand = new DelegateCommand<object>(DeleteItem);
            TextChangedCommand = new DelegateCommand<string>(TextChanged);
            SelectItemFromPopupCommand = new DelegateCommand<object>(SelectItemFromPopup,
                                                                     (x) => SelectedObjectFromPopup != null);
            BackSpacePressedCommand = new DelegateCommand<string>(BackSpacePressed);

            ButtonDeleteMouseEnterCommand=new DelegateCommand(ButtonDeleteMouseEnter);
            ButtonDeleteMouseLeaveCommand=new DelegateCommand(ButtonDeleteMouseLeave);
        }

        #region DependencyProperties

        public static readonly DependencyProperty TagViewModelCollectionProperty =
            DependencyProperty.Register("TagViewModelCollection", typeof (ObservableCollection<TagControlItemViewModel>),
                                        typeof (TagControl),
                                        new PropertyMetadata(new ObservableCollection<TagControlItemViewModel>()));


        public static readonly DependencyProperty SearchedItemsCollectionProperty =
            DependencyProperty.Register("SearchedItemsCollection", typeof (object), typeof (TagControl));

        public static readonly DependencyProperty SelectedObjectFromPopupProperty = DependencyProperty.Register(
            "SelectedObjectFromPopup", typeof (object), typeof (TagControl));

        public static readonly DependencyProperty IsSearchEnabledProperty =
            DependencyProperty.Register("IsSearchEnabled", typeof (bool), typeof (TagControl),
                                        new PropertyMetadata(false));

        public static readonly DependencyProperty InputTextForSearchProperty =
            DependencyProperty.Register("InputTextForSearch",
                                        typeof (string),
                                        typeof (TagControl),
                                        new PropertyMetadata(
                                            string.Empty));

        public static readonly DependencyProperty ItemTemplateForPopupProperty = DependencyProperty.Register(
            "ItemTemplateForPopup", typeof (DataTemplate), typeof (TagControl));

        public static readonly DependencyProperty TemplateForItemListProperty =
            DependencyProperty.Register("TemplateForItemList", typeof (DataTemplate), typeof (TagControl));

        public static readonly DependencyProperty IsTextForSearchEmptyProperty =
            DependencyProperty.Register("IsTextForSearchEmpty", typeof (bool), typeof (TagControl),
                                        new PropertyMetadata(true));


        public static readonly DependencyProperty IsControlInitializedProperty =
            DependencyProperty.Register("IsControlInitialized", typeof (bool), typeof (TagControl),
                                        new PropertyMetadata(false));

        
        #endregion

        #region Properties

        public ObservableCollection<TagControlItemViewModel> TagViewModelCollection
        {
            get { return (ObservableCollection<TagControlItemViewModel>) GetValue(TagViewModelCollectionProperty); }
            set { SetValue(TagViewModelCollectionProperty, value); }
        }

        public object SearchedItemsCollection
        {
            get { return (object) GetValue(SearchedItemsCollectionProperty); }
            set { SetValue(SearchedItemsCollectionProperty, value); }
        }

        public object SelectedObjectFromPopup
        {
            get { return (object) GetValue(SelectedObjectFromPopupProperty); }
            set { SetValue(SelectedObjectFromPopupProperty, value); }
        }

       
        public bool IsSearchEnabled
        {
            get { return (bool) GetValue(IsSearchEnabledProperty); }
            set { SetValue(IsSearchEnabledProperty, value); }
        }

        public string InputTextForSearch
        {
            get { return (string) GetValue(InputTextForSearchProperty); }
            set { SetValue(InputTextForSearchProperty, value); }
        }

        public DataTemplate ItemTemplateForPopup
        {
            get { return (DataTemplate) GetValue(ItemTemplateForPopupProperty); }
            set { SetValue(ItemTemplateForPopupProperty, value); }
        }

        public DataTemplate TemplateForItemList
        {
            get { return (DataTemplate) GetValue(TemplateForItemListProperty); }
            set { SetValue(TemplateForItemListProperty, value); }
        }

        public bool IsTextForSearchEmpty
        {
            get { return (bool) GetValue(IsTextForSearchEmptyProperty); }
            set { SetValue(IsTextForSearchEmptyProperty, value); }
        }

        public bool IsControlInitialized
        {
            get { return (bool) GetValue(IsControlInitializedProperty); }
            set { SetValue(IsControlInitializedProperty, value); }
        }

        #endregion

        #region private methods

        private CustomerModels.Label CreateLabel(string name, string description = "description")
        {
            CustomerModels.Label result=new CustomerModels.Label();

            result.Name = name;
            result.Description = description;

            return result;
        }

        #endregion

        #region Events

        public event EventHandler RefreshSearchItemsEvent;

        #endregion

        #region Commands

        public DelegateCommand<string> SpacePressedCommand { get; private set; }
        public DelegateCommand<object> DeleteItemCommand { get; private set; }
        public DelegateCommand<string> TextChangedCommand { get; private set; }
        public DelegateCommand<object> SelectItemFromPopupCommand { get; private set; }
        public DelegateCommand<string> BackSpacePressedCommand { get; private set; }

        public DelegateCommand ButtonDeleteMouseEnterCommand { get; private set; }
        public DelegateCommand ButtonDeleteMouseLeaveCommand { get; private set; }

        #endregion

        #region Command Implementation

        private void SpacePressed(string inputText)
        {
            if (IsSearchEnabled)
            {
               return;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(inputText))
                {
                    CustomerModels.Label newLabel = CreateLabel(inputText);
                    TagViewModelCollection.Add(new TagControlItemViewModel(newLabel) {IsEditing = false});

                    var editViewModel = TagViewModelCollection.SingleOrDefault(item => item.IsEditing == true);
                    if (editViewModel != null)
                    {
                        (editViewModel.InnerItem as CustomerModels.Label).Name = string.Empty;
                    }

                    var view = CollectionViewSource.GetDefaultView(TagViewModelCollection);
                    view.SortDescriptions.Add(new SortDescription("IsEditing", ListSortDirection.Ascending));
                    view.Refresh();
                }
            }


        }

        private void DeleteItem(object item)
        {
            if (item == null)
                return;

            var itemToDelete = item as TagControlItemViewModel;
            TagViewModelCollection.Remove(itemToDelete);

        }

        private void TextChanged(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                IsTextForSearchEmpty = true;
                return;
            }


            if (IsSearchEnabled)
            {
                IsControlInitialized = true;
                IsTextForSearchEmpty = false;

                if (RefreshSearchItemsEvent != null)
                {
                    RefreshSearchItemsEvent(this, EventArgs.Empty);
                    popupSearch.IsOpen = true;
                }


            }
        }

        private void SelectItemFromPopup(object selectedItem)
        {
            if (selectedItem == null)
                return;

            TagViewModelCollection.Add(new TagControlItemViewModel(selectedItem) { IsEditing = false });

            popupSearch.IsOpen = false;
            InputTextForSearch = string.Empty;
            IsTextForSearchEmpty = true;
            SelectedObjectFromPopup = null;

            var view = CollectionViewSource.GetDefaultView(TagViewModelCollection);
            view.SortDescriptions.Add(new SortDescription("IsEditing", ListSortDirection.Ascending));
            view.Refresh();
        }

        private void BackSpacePressed(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                var lastItem = TagViewModelCollection.LastOrDefault(tvm => tvm.IsEditing == false);
                if (lastItem != null)
                {
                    TagViewModelCollection.Remove(lastItem);
                }
            }
            else
            {
                TagControlItemViewModel itemWithEDit = null;
                foreach (var item in lst.Items)
                {
                    if ((item as TagControlItemViewModel).IsEditing)
                    {
                        itemWithEDit = item as TagControlItemViewModel;
                    }
                }

                if (itemWithEDit != null)
                {

                    var item = lst.ItemContainerGenerator.ContainerFromItem(itemWithEDit) as ListBoxItem;
                    if (item != null)
                    {
                        var txt = UIHelper.FindVisualChild<TextBox>(item);
                        if (txt != null)
                        {
                            string currentValue = txt.Text;
                            string selectedText = txt.SelectedText;

                            if (!string.IsNullOrEmpty(selectedText))
                            {
                                //delete all selected text
                                var caretIndex = txt.CaretIndex;
                                txt.Text=txt.Text.Replace(txt.SelectedText, string.Empty);
                                txt.CaretIndex = caretIndex;
                            }
                            else
                            {
                                //delete last symbol in textbox
                                var caretIndex = txt.CaretIndex;

                                if (caretIndex > 0)
                                {
                                    string backSpace = currentValue.Remove(caretIndex - 1, 1);
                                    txt.Text = backSpace;
                                    txt.CaretIndex = caretIndex - 1;
                                }
                            }
                        }

                    }
                }
            }

        }


        private void ButtonDeleteMouseEnter()
        {
            this.PreviewMouseUp -= TagControl_OnPreviewMouseUp;
        }

        private void ButtonDeleteMouseLeave()
        {
            this.PreviewMouseUp += TagControl_OnPreviewMouseUp;
        }


        #endregion

        #region Handlers

        private void TagControl_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TagControlItemViewModel itemWithEDit = null;
            foreach (var item in lst.Items)
            {
                if ((item as TagControlItemViewModel).IsEditing)
                {
                    itemWithEDit = item as TagControlItemViewModel;
                }
            }

            if (itemWithEDit != null)
            {

                var item = lst.ItemContainerGenerator.ContainerFromItem(itemWithEDit) as ListBoxItem;
                if (item != null)
                {
                    item.Focus();
                    var txt = UIHelper.FindVisualChild<TextBox>(item);
                    if (txt != null)
                    {
                        txt.Focus();
                    }
                }

            }
        }

        #endregion

    }


    public class TagControlItemViewModel:ViewModelBase
    {
        #region Constructor

        public TagControlItemViewModel(object item)
        {
            InnerItem = item;
        }

        #endregion

        #region Properties

        private object _innerItem;
        public object InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value;
            OnPropertyChanged();}
        }

        #endregion

    }
    

   

}
