using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using VirtoCommerce.ManagementClient.Core.Controls;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class RequiredFieldHelper
    {

       

        #region Dependency Property

        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.RegisterAttached("IsRequired",
                                                                                                           typeof (bool),
                                                                                                           typeof (
                                                                                                               RequiredFieldHelper
                                                                                                               ),
                                                                                                           new PropertyMetadata
                                                                                                               (
                                                                                                               false,
                                                                                                               OnIsReqiredChanged));


        #endregion

        public static void SetIsRequired(DependencyObject depObj, bool value)
        {
            depObj.SetValue(IsRequiredProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        [AttachedPropertyBrowsableForType(typeof(WatermarkedTextBox))]
        public static bool GetIsRequired(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(IsRequiredProperty);
        }



        private static void OnIsReqiredChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsInDesignMode)
                return;

            Control control = sender as Control;

            if (control == null)
                return;
        }



        private static bool IsInDesignMode
        {
            get
            {
                return (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty
                                                                        , typeof (DependencyObject))
                                                          .Metadata.DefaultValue;
            }
        }

    }
}
