using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
    public static class TreeViewSelectedItemBehavior
    {
        public static readonly DependencyProperty SelectedItemChangedHandlerProperty =
            DependencyProperty.RegisterAttached("SelectedItemChangedHandler",
                typeof(ICommand),
                typeof(TreeViewSelectedItemBehavior),
                new FrameworkPropertyMetadata(OnSelectedItemChangedHandlerChanged));


        public static ICommand GetSelectedItemChangedHandler(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return element.GetValue(SelectedItemChangedHandlerProperty) as ICommand;
        }

        public static void SetSelectedItemChangedHandler(DependencyObject element, ICommand value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(SelectedItemChangedHandlerProperty, value);
        }

        public static void OnSelectedItemChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            TreeView treeView = (TreeView)d;

            if (e.OldValue == null && e.NewValue != null)
            {
                treeView.SelectedItemChanged += ItemsControl_SelectionChanged;
            }

            if (e.OldValue != null && e.NewValue == null)
            {
                treeView.SelectedItemChanged -= ItemsControl_SelectionChanged;
            }
        }


        public static void ItemsControl_SelectionChanged(Object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            TreeView treeView = (TreeView)sender;

            ICommand itemsChangedHandler = GetSelectedItemChangedHandler(treeView);

            itemsChangedHandler.Execute(treeView.SelectedItem);
        }
    }

}
