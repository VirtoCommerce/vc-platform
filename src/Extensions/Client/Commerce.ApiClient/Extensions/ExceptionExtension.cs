using System;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ExceptionExtension
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Determines whether [is] [the specified ex].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The ex.</param>
        /// <returns><c>true</c> if [is] [the specified ex]; otherwise, <c>false</c>.</returns>
        public static bool Is<T>(this Exception ex) where T : Exception
        {
            if (ex is T)
            {
                return true;
            }
            if (ex.InnerException != null)
            {
                return Is<T>(ex.InnerException);
            }
            return false;
        }

        #endregion
    }
}
