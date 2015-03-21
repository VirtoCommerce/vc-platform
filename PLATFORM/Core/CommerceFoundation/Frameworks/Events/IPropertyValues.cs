using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Events
{
    public interface IPropertyValues
    {
        IEnumerable<string> PropertyNames {get;}
        object this[string propertyName] {get;}
        TValue GetValue<TValue>(string propertyName);
    }
}
