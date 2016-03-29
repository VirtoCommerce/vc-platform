using System;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// Math filters allow you to apply mathematical tasks
    /// https://docs.shopify.com/themes/liquid/filters/math-filters
    /// </summary>
    public class MathFilters
    {
        /// <summary>
        /// Divides an output by a number. The output is rounded down to the nearest integer.
        /// https://docs.shopify.com/themes/liquid/filters/math-filters#divided_by
        /// </summary>
        /// <param name="dividend">Dividend</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Value rounded down to the nearest integer</returns>
        public static long DividedBy(long dividend, long divisor)
        {
            return (long)(Math.Floor((double)(dividend / divisor)));
        }

        /// <summary>
        /// Adds a number to an output
        /// https://docs.shopify.com/themes/liquid/filters/math-filters#plus
        /// </summary>
        /// <param name="summand1">First summand</param>
        /// <param name="summand2">Second summand</param>
        /// <returns>Summ of summands</returns>
        public static double Plus(double summand1, double summand2)
        {
            return summand1 + summand2;
        }

        /// <summary>
        /// Subtracts a number from an output
        /// https://docs.shopify.com/themes/liquid/filters/math-filters#minus
        /// </summary>
        /// <param name="minuend">Minuend</param>
        /// <param name="subtrahend">Subtrahend</param>
        /// <returns></returns>
        public static double Minus(double minuend, double subtrahend)
        {
            return minuend - subtrahend;
        }
    }
}