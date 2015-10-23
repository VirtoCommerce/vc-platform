using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Operators
{
    public static class CommonOperators
    {
        public static bool ContainsMethod(object left, object right)
        {
            if (left is string)
            {
                return right != null && ((string)left).Contains(right as string);
            }

            if (left is IList)
            {
                return ((IList)left).Contains(right);
            }

            return false;
        }
    }
}

