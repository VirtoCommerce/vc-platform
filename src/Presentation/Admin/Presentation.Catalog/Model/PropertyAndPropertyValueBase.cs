using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Model
{
    public class PropertyAndPropertyValueBase : INotifyPropertyChanged
    {
        public Property Property { get; set; }

        PropertyValueBase _value;
        [DoNotNotify]
        public PropertyValueBase Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); OnPropertyChanged("IsValid"); }
        }

        ObservableCollection<PropertyValueBase> _values;
        public ObservableCollection<PropertyValueBase> Values
        {
            get { return _values; }
            set { _values = value; OnPropertyChanged(); }
        }

        public string Locale { get; set; }

        public bool IsMultiValue
        {
            get { return Property != null && Property.IsMultiValue; }
        }

        public bool IsEnum
        {
            get { return Property != null && Property.IsEnum; }
        }

        public string PropertyName
        {
            get
            {
                if (Property != null)
                    return Property.Name;

                if (Value != null)
                    return Value.Name;

                throw new ArgumentNullException("PropertyName");
            }
        }

        public int PropertyValueType
        {
            get
            {
                if (Property != null)
                    return Property.PropertyValueType;

                if (Value != null)
                    return Value.ValueType;

                throw new ArgumentNullException("PropertyValueType");
            }
        }

        public bool IsValid
        {
            get
            {
                return (Value != null && (!string.IsNullOrEmpty(Value.ToString()) || (IsEnum && !string.IsNullOrEmpty(Value.KeyValue))))
                                  || (Values != null && Values.Any())
                                  || !Property.IsRequired;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
