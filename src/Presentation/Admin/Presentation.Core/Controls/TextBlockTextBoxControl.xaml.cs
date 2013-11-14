using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Interaction logic for TextBlockTextBoxControl.xaml
    /// </summary>
    public partial class TextBlockTextBoxControl : UserControl, INotifyPropertyChanged 
    {
        public TextBlockTextBoxControl()
        {
            InitializeComponent();

            txtReadOnly.MouseDown += txtReadOnly_MouseDown;
        }

        #region Handlers

        void txtReadOnly_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtReadOnly.Visibility=Visibility.Collapsed;
            txtEdit.Visibility = Visibility.Visible;
            EditedText = Text;
            txtEdit.Focus();
        }

        #endregion

        #region UserControl overrided methods

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            txtReadOnly.Visibility = Visibility.Visible;
            txtEdit.Visibility = Visibility.Collapsed;
            Text = EditedText;
            base.OnLostKeyboardFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            txtReadOnly.Visibility = Visibility.Visible;
            txtEdit.Visibility = Visibility.Collapsed;
            Text = EditedText;
            base.OnLostFocus(e);
        }

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string),
                                                                                             typeof (
                                                                                                 TextBlockTextBoxControl
                                                                                                 ),
                                                                                             new PropertyMetadata(
                                                                                                 string.Empty));


        public static readonly DependencyProperty EditedTextProperty = DependencyProperty.Register("EditedText",
                                                                                                   typeof (string),
                                                                                                   typeof (
                                                                                                       TextBlockTextBoxControl
                                                                                                       ),
                                                                                                   new PropertyMetadata(
                                                                                                       string.Empty));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
                                                                                                  typeof (string),
                                                                                                  typeof (
                                                                                                      TextBlockTextBoxControl
                                                                                                      ),
                                                                                                  new PropertyMetadata(
                                                                                                      string.Empty));

      
        #endregion

        #region Public Property

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set{SetValue(TextProperty,value);}
        }

        public string EditedText
        {
            get { return (string) GetValue(EditedTextProperty); }
            set { SetValue(EditedTextProperty, value); }
        }

        public string Watermark
        {
            get { return (string) GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
