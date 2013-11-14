using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtoCommerce.ManagementClient.Core.Controls.ViewModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Interaction logic for VirtualFolderTree.xaml
    /// </summary>
    public partial class VirtualFolderTree : TreeView
    {

        public static readonly DependencyProperty SelectedFolderModelProperty =
                               DependencyProperty.Register("SelectedFolderModel", typeof(IViewModel), typeof(VirtualFolderTree),
                               new FrameworkPropertyMetadata());

        public IViewModel SelectedFolderModel
        {
            get
            {
                return (IViewModel)GetValue(SelectedFolderModelProperty);
            }
            set
            {
                SetValue(SelectedFolderModelProperty, value);
            }
        }

        public VirtualFolderTree()
        {
            InitializeComponent();


            RoutedEventHandler selectionDelegate = (sender, args) =>
            {
                var selectedItem = SelectedItem as IViewModel;
                if (selectedItem != null)
                {
                    SelectedFolderModel = selectedItem;
                    if (args.OriginalSource is TreeViewItem)
                    {
                        ((TreeViewItem)args.OriginalSource).BringIntoView();
                    }
                }
            };

            this.AddHandler(TreeViewItem.SelectedEvent, selectionDelegate);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        #region Handlers

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        #endregion

        #region StaticHelpers

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }


        #endregion
    }
}
