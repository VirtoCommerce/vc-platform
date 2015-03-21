using System;

namespace VirtoCommerce.Foundation.Data.Common
{
	public static class GuidExtension
	{
        /// <summary>
        /// To the base64.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
		public static string ToBase64(this Guid instance)
		{
			string retVal = Convert.ToBase64String(instance.ToByteArray());
			retVal = retVal.Replace("/", "_").Replace("+", "-");
			return retVal.Substring(0, 22);
		}

        /// <summary>
        /// Base64s to GUID.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
		public static Guid Base64ToGuid(this string instance)
		{
			var retVal = instance.Replace("_", "/").Replace("-", "+");
			byte[] buffer = Convert.FromBase64String(instance + "==");
			return new Guid(buffer);
		}
	}
}
