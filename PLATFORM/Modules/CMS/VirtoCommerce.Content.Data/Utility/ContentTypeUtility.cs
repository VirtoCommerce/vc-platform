using System;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Content.Data.Utility
{
	public static class ContentTypeUtility
	{
		public static bool IsImageContentType(string contentType)
		{
			return contentType.Equals("image/bmp") ||
				contentType.Equals("image/png") ||
				contentType.Equals("image/jpeg") ||
				contentType.Equals("image/gif");
		}

		public static bool IsTextContentType(string contentType)
		{
			return contentType.Equals("text/html") ||
				contentType.Equals("application/javascript") ||
				contentType.Equals("application/json");
		}

	
	}
}

