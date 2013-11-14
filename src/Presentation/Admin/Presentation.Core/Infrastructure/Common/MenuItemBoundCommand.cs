using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    /// <summary>
    /// Comamnd parameter binding bug in ContextMenu fix (workaround). Solution is based on http://stackoverflow.com/questions/3027224/icommand-canexecute-being-passed-null-even-though-commandparameter-is-set
    /// </summary>
    public static class MenuItemBoundCommand
    {
        public static object GetParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(ParameterProperty);
        }

        public static void SetParameter(DependencyObject obj, object value)
        {
            obj.SetValue(ParameterProperty, value);
        }

        public static readonly DependencyProperty ParameterProperty = DependencyProperty.RegisterAttached("Parameter", typeof(object), typeof(MenuItemBoundCommand), new UIPropertyMetadata(null, ParameterChanged));

        private static void ParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // var button = d as System.Windows.Controls.Primitives.ButtonBase;
            var button = d as System.Windows.Controls.MenuItem;
            if (button == null)
            {
                return;
            }

            button.CommandParameter = e.NewValue;
            var cmd = button.Command as DelegateCommandBase;
            if (cmd != null)
            {
                cmd.RaiseCanExecuteChanged();
            }
        }
    }
}
