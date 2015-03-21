using System;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class NumberExtensions
    {
        public static double DigitCount(this int num)
        {
            return Math.Floor(Math.Log10(num) + 1);
        }
    }
}
