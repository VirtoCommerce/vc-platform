#region Usings

using System.Windows;
using System.Windows.Controls;
using VirtoCommerce.ManagementClient.Core.Controls;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    public class InteractionDialogWindow : Window
    {
        #region Dependency Properties
        public static readonly DependencyProperty DialogWindowContentProperty =
            DependencyProperty.Register(
                "DialogWindowContent", 
                typeof(object), 
                typeof(InteractionDialogWindow),
                new PropertyMetadata(default(UserControl)));
        #endregion

        #region Properties
        public object DialogWindowContent
        {
            get { return GetValue(DialogWindowContentProperty); }
            set { SetValue(DialogWindowContentProperty, value); }
        }
        #endregion
    }
}
