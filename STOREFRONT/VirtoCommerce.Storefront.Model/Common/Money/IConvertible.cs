using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IConvertible<T>
    {
        T ConvertTo(Currency currency);
    }
}
