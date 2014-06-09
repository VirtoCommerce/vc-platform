using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
    public static class NumberExtensions
    {
        public static double DigitCount(this int num)
        {
            return Math.Floor(Math.Log10(num) + 1);
        }
    }
}
