using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Events;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
    public class PropertyValues : IPropertyValues
    {
        public IEnumerable<string> PropertyNames 
        { 
            get
            {
                return _values.PropertyNames;
            }
        
        }
        public object this[string propertyName] 
        { 
            get
            {
                return _values[propertyName];
            }

        }
        public TValue GetValue<TValue>(string propertyName)
        {
            return (TValue)this[propertyName];
        }

        DbPropertyValues _values = null;
        public PropertyValues(DbPropertyValues values)
        {
            _values = values;
        }
    }
}
