using System.Collections;
using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Operators
{
    public static class CommonOperators
    {
        public static bool ContainsMethod(object left, object right)
        {
            var liquidContains = left as ILiquidContains;
            if (liquidContains != null)
            {
                return liquidContains.Contains(right);
            }

            var leftString = left as string;
            if (leftString != null)
            {
                var rightString = right as string;
                return rightString != null && leftString.Contains(rightString);
            }

            var list = left as IList;
            if (list != null)
            {
                return list.Contains(right);
            }

            return false;
        }
    }
}
