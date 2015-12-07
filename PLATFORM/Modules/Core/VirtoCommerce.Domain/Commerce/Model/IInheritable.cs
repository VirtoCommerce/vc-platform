using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Commerce.Model
{
    /// <summary>
    /// Used for mark types which can be inherited form parent object (Product -> Variations  example)
    /// </summary>
    public interface IInheritable
    {
        bool IsInherited { get; }
    }
}
