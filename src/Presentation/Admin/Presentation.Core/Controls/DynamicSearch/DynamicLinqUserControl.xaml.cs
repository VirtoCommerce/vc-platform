using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Interaction logic for DynamicLinqUserControl.xaml
    /// </summary>
    public partial class DynamicLinqUserControl : UserControl
    {
        public DynamicLinqUserControl()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var vm = DataContext as ExpressionList;
            var param = e.Parameter as ExpressionVM;
            if (vm != null && param != null) vm.Remove(param);
        }
                
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ExpressionList;
            if (vm != null)
            {
                var param = new ExpressionVM();
                vm.Add(param);
            }
        }
    }
}
