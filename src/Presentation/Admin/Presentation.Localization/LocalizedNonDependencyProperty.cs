using System;
using System.Reflection;
using System.Windows;
using System.Threading;

namespace VirtoCommerce.ManagementClient.Localization
{
    public class LocalizedNonDependencyProperty : LocalizedProperty
    {
        public LocalizedNonDependencyProperty(DependencyObject obj, PropertyInfo property)
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
                return ((PropertyInfo)Property).GetValue(targetObject, null);
            }
            else
            {
                return null;
            }
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
                    ((PropertyInfo)Property).SetValue(targetObject, value, null);
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
            return ((PropertyInfo)Property).PropertyType;
        }
    }
}
