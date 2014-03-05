using System;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Localization
{
    public class LocalizedDependencyProperty : LocalizedProperty
    {
        public LocalizedDependencyProperty(DependencyObject obj, DependencyProperty property)
            : base(obj, property)
        {
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>The value of the property.</returns>
        internal protected override object GetValue()
        {
            var targetObject = Object;

            if (targetObject != null)
            {
                if (targetObject.CheckAccess())
                {
                    return targetObject.GetValue((DependencyProperty)Property);
                }
                else
                {
                    return targetObject.Dispatcher.Invoke(new DispatcherOperationCallback(GetValue));
                }
            }

            return null;
        }

        object GetValue(object dummy)
        {
            return GetValue();
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">The value to set.</param>
        internal protected override void SetValue(object value)
        {
            var targetObject = Object;

            if (targetObject != null)
            {
                if (targetObject.CheckAccess())
                {
                    targetObject.SetValue((DependencyProperty)Property, value);
                }
                else
                {
                    targetObject.Dispatcher.Invoke(new SendOrPostCallback(SetValue), value);
                }
            }
        }

        /// <summary>
        /// Gets the type of the value of the property.
        /// </summary>
        /// <returns>The type of the value of the property.</returns>
        internal protected override Type GetValueType()
        {
            return ((DependencyProperty)Property).PropertyType;
        }
    }
}
