using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    public partial class ExpandedTabControl : System.Windows.Controls.TabControl
    {

        public DelegateCommand<object> HeaderClickCommand { get; set; }
        public ExpandedTabControl()
            : base()
        {
            IsExpanded = true;
            HeaderClickCommand = new DelegateCommand<object>(RaiseHeaderClickRequest);
        }

        private void RaiseHeaderClickRequest(object tabItem)
        {
            IsExpanded = !(IsExpanded && tabItem is TabItem && (tabItem as TabItem).IsSelected);
        }

        private static readonly DependencyPropertyKey IsExpandedPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsExpanded",
                typeof(bool),
                typeof(ExpandedTabControl),
                new PropertyMetadata());
        public static DependencyProperty IsExpandedProperty = IsExpandedPropertyKey.DependencyProperty;

        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }
            private set
            {
                SetValue(IsExpandedPropertyKey, value);
            }
        }

        public void Collapse()
        {
            IsExpanded = false;
        }
    }
}
