using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using Xceed.Wpf.Toolkit;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class BindingExtensions
    {
        public static Binding Clone(this Binding binding)
        {
            var cloned = new Binding();
            //copy properties here
            cloned.AsyncState = binding.AsyncState;
            cloned.BindingGroupName = binding.BindingGroupName;
            cloned.BindsDirectlyToSource = binding.BindsDirectlyToSource;
            cloned.Converter = binding.Converter;
            cloned.ConverterCulture = binding.ConverterCulture;
            cloned.ConverterParameter = binding.ConverterParameter;
            cloned.Delay = binding.Delay;
            
            cloned.FallbackValue = binding.FallbackValue;
            cloned.IsAsync = binding.IsAsync;
            cloned.Mode = BindingMode.TwoWay;
            cloned.NotifyOnSourceUpdated = binding.NotifyOnSourceUpdated;
            cloned.NotifyOnTargetUpdated = binding.NotifyOnTargetUpdated;
            cloned.NotifyOnValidationError = binding.NotifyOnValidationError;
            cloned.Path = binding.Path;
            
            cloned.StringFormat = binding.StringFormat;
            cloned.TargetNullValue = binding.TargetNullValue;
            cloned.UpdateSourceExceptionFilter = binding.UpdateSourceExceptionFilter;
            cloned.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cloned.ValidatesOnDataErrors = binding.ValidatesOnDataErrors;
            cloned.ValidatesOnExceptions = binding.ValidatesOnExceptions;
            cloned.ValidatesOnNotifyDataErrors = binding.ValidatesOnNotifyDataErrors;
            cloned.XPath = binding.XPath;

            
            cloned.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (UserControl), 1);
          

            return cloned;
        }
    }




    public static class BindingFromStringBehavior
    {

        #region DependencyProperty

        public static readonly DependencyProperty BindingStringProperty =
            DependencyProperty.RegisterAttached("BindingString",
                                                typeof (string), typeof (BindingFromStringBehavior),
                                                new PropertyMetadata(string.Empty, OnBindingStringChanged));

        #endregion



        #region Setter and Getter

        public static void SetBindingString(DependencyObject depObj, string value)
        {
            depObj.SetValue(BindingStringProperty,value);
        }

        public static string GetBindingString(DependencyObject depObj)
        {
            return (string)depObj.GetValue(BindingStringProperty);
        }

        #endregion


        private static void OnBindingStringChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control element = sender as Control;
            BindingExpression bindingFromElement = null;
            if (element is TextBox)
            {
                bindingFromElement = element.GetBindingExpression(TextBox.TextProperty);
            }
            else if (element is DatePicker)
            {
                bindingFromElement = element.GetBindingExpression(DatePicker.SelectedDateProperty);
            }
            else if (element is IntegerUpDown)
            {
                bindingFromElement = element.GetBindingExpression(IntegerUpDown.ValueProperty);
            }
            else if (element is CheckBox)
            {
                bindingFromElement = element.GetBindingExpression(CheckBox.IsCheckedProperty);
            }

            if (bindingFromElement == null)
                return;

            Binding binding = bindingFromElement.ParentBinding;
            Binding newBinding = binding.Clone();

            newBinding.Path = new PropertyPath(e.NewValue.ToString());

            if (element is TextBox)
            {
                BindingOperations.SetBinding(element, TextBox.TextProperty, newBinding);
                bindingFromElement = element.GetBindingExpression(TextBox.TextProperty);
            }
            else if (element is DatePicker)
            {
                BindingOperations.SetBinding(element, DatePicker.SelectedDateProperty, newBinding);
                bindingFromElement = element.GetBindingExpression(DatePicker.SelectedDateProperty);
            }
            else if(element is IntegerUpDown)
            {
                BindingOperations.SetBinding(element, IntegerUpDown.ValueProperty, newBinding);
                bindingFromElement = element.GetBindingExpression(IntegerUpDown.ValueProperty);
            }
            else if (element is CheckBox)
            {
                BindingOperations.SetBinding(element, CheckBox.IsCheckedProperty, newBinding);
                bindingFromElement = element.GetBindingExpression(CheckBox.IsCheckedProperty);
            }
            
            bindingFromElement.UpdateTarget();
        }

        

    }
}
