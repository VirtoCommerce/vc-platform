using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Localization
{
    /// <summary>
    /// Contains information about a localized property.
    /// </summary>
    public abstract class LocalizedProperty
    {
        #region Public properties

        /// <summary>
        /// The convert to use to convert the retrieved resource to a value suitable
        /// for the property.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// The parameter to pass to the converter.
        /// </summary>
        public object ConverterParameter { get; set; }

        #endregion

        #region Protected properties

        /// <summary>
        /// The object to which the property belongs.
        /// </summary>
        internal protected DependencyObject Object
        {
            get
            {
                return (DependencyObject)_object.Target;
            }
        }

        /// <summary>
        /// The localized property.
        /// </summary>
        internal protected object Property { get; private set; }

        #endregion

        #region Internal properties

        /// <summary>
        /// Indicates if the object to the property belongs has been garbage collected.
        /// </summary>
        internal bool IsAlive
        {
            get
            {
                return _object.IsAlive;
            }
        }

        /// <summary>
        /// Indicates if the object is in design mode.
        /// </summary>
        internal bool IsInDesignMode
        {
            get
            {
                var obj = _object.Target as DependencyObject;

                return obj != null && DesignerProperties.GetIsInDesignMode(obj);
            }
        }

        #endregion

        #region Fields

        WeakReference _object;

        int _hashCode;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectValue"/> class.
        /// </summary>
        /// <param name="targetObject">The owner of the property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is <c>null</c>.</exception>
        protected LocalizedProperty(DependencyObject obj, object property)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            _object = new WeakReference(obj);

            Property = property;

            _hashCode = obj.GetHashCode() ^ property.GetHashCode();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>The value of the property.</returns>
        internal protected abstract object GetValue();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">The value to set.</param>
        internal protected abstract void SetValue(object value);

        /// <summary>
        /// Gets the type of the value of the property.
        /// </summary>
        /// <returns>The type of the value of the property.</returns>
        internal protected abstract Type GetValueType();

        #endregion

        #region Attached properties


        /// <summary>
        /// Gets the culture set for the object.
        /// </summary>
        /// <returns>
        /// A <see cref="CultureInfo"/> or null if no explicit value is set for the object.
        /// </returns>
        public CultureInfo GetCulture()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            CultureInfo result;

            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetCulture(obj);
            }
            else
            {
                result = (CultureInfo)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetCulture((DependencyObject)x)), obj);
            }

            if (result == null)
            {
                // Get the culture of the UI thread in case the current thread is different
                result = obj.Dispatcher.Thread.CurrentCulture;
            }

            return result;
        }

        /// <summary>
        /// Gets the UI culture set for the object.
        /// </summary>
        /// <returns>
        /// A <see cref="CultureInfo"/> or null if no explicit value is set for the object.
        /// </returns>
        public CultureInfo GetUICulture()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            CultureInfo result;

            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetUICulture(obj);
            }
            else
            {
                result = (CultureInfo)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetUICulture((DependencyObject)x)), obj);
            }

            if (result == null)
            {
                // Get the culture of the UI thread in case the current thread is different
                result = obj.Dispatcher.Thread.CurrentUICulture;
            }

            return result;
        }

        /// <summary>
        /// Gets the Category set for the object.
        /// </summary>
        public string GetCategory()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            string result;
            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetCategory(obj);
            }
            else
            {
                result = (string)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetCategory((DependencyObject)x)), obj);
            }

            if (string.IsNullOrEmpty(result))
            {
                // Return the default value
                result = LocalizationScope.DefaultCategory;
            }

            return result;
        }

        #endregion

        #region Hash code & equals

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            var instance = obj as LocalizedDependencyProperty;

            if (instance == null)
            {
                return false;
            }

            if (_hashCode != instance._hashCode)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var targetObject = Object;

            return targetObject != null
                && object.ReferenceEquals(targetObject, instance.Object)
                && object.ReferenceEquals(Property, instance.Property);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
