using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public class PasswordboxWatermarkTextHelper : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordboxWatermarkTextHelper),
                                                new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static int GetTextLength(DependencyObject obj)
        {
            return (int) obj.GetValue(TextLengthProperty);
        }

        public static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);
            if (value >= 1)
                obj.SetValue(HasTextProperty, true);
            else obj.SetValue(HasTextProperty, false);
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(PasswordboxWatermarkTextHelper),
                                                new UIPropertyMetadata(0));

        private static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(PasswordboxWatermarkTextHelper),
                                                new FrameworkPropertyMetadata(false));

        public bool HasText
        {
            get { return (bool) GetValue(HasTextProperty); }
            set { SetValue(HasTextProperty, value); }
        }

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                TextBox txtBox = d as TextBox;
                if ((bool) e.NewValue)
                    txtBox.TextChanged += TextChanged;
                else txtBox.TextChanged -= TextChanged;
            }
            else if (d is PasswordBox)
            {
                PasswordBox passBox = d as PasswordBox;
                if ((bool) e.NewValue)
                    passBox.PasswordChanged += PasswordChanged;
                else passBox.PasswordChanged -= PasswordChanged;
            }
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox == null) return;
            SetTextLength(txtBox, txtBox.Text.Length);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passBox = sender as PasswordBox;
            if (passBox == null) return;
            SetTextLength(passBox, passBox.Password.Length);
        }
    }
}
