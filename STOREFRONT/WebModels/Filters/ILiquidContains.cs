using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Filters
{
    public interface ILiquidContains
    {
        bool Contains(object value);
    }
}
