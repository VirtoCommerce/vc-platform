using System;

namespace VirtoCommerce.Web.Client.Extensions
{
	/// <summary>
	/// Class ExceptionExtensions.
	/// </summary>
    public static class ExceptionExtensions
    {
		/// <summary>
		/// Determines whether [is] [the specified ex].
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
    }
}