using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithSearchListControl.xaml
    /// </summary>
    public partial class TextBoxWithSearchListControl : UserControl
    {
        public TextBoxWithSearchListControl()
        {
            InitializeComponent();
            txtInput.TextChanged += txtInput_TextChanged;
        }

        #region Depecndency property

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string),
                                                                                             typeof (
                                                                                                 TextBoxWithSearchListControl
                                                                                                 ),
                                                                                             new PropertyMetadata(
                                                                                                 string.Empty));

        public static readonly DependencyProperty SearchedItemsProperty = DependencyProperty.Register("SearchedItems",
                                                                                                      typeof (ObservableCollection<Contact>),
                                                                                                      typeof (
                                                                                                          TextBoxWithSearchListControl
                                                                                                          ));

        public static readonly DependencyProperty SelectedItemFromPopupProperty =
            DependencyProperty.Register("SelectedItemFromPopup", typeof (Contact), typeof (TextBoxWithSearchListControl));



        #endregion


        #region Public Properties

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ObservableCollection<Contact> SearchedItems
        {
            get { return (ObservableCollection<Contact>)GetValue(SearchedItemsProperty); }
            set { SetValue(SearchedItemsProperty, value); }
        }

        public Contact SelectedItemFromPopup
        {
            get { return (Contact) GetValue(SelectedItemFromPopupProperty); }
            set { SetValue(SelectedItemFromPopupProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler RefreshSearchItemsEvent;

        #endregion

        #region Handlers

        void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = txtInput.Text;
            if (inputText.Length == 0)
            {
                popupSearch.IsOpen = false;
                return;
            }

            if (RefreshSearchItemsEvent != null)
            {
                RefreshSearchItemsEvent(this, EventArgs.Empty);
                popupSearch.IsOpen = true;
            }
        }

        private void LstItems_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItemFromPopup != null)
            {

                string contactName = SelectedItemFromPopup.FullName;
                txtInput.TextChanged -= txtInput_TextChanged;
                txtInput.Text = contactName;
                popupSearch.IsOpen = false;
                SelectedItemFromPopup = null;
                txtInput.TextChanged += txtInput_TextChanged;
            }
        }

        #endregion


        #region Private Methods



        #endregion

        
    }
}
