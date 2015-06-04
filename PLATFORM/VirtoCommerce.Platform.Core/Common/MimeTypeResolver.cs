using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class MimeTypeResolver
	{
		private static Dictionary<string, string> _mapping;
		static MimeTypeResolver()
		{
			_mapping = new Dictionary<string, string>();
			_mapping.Add("pdf", "application/pdf");
			_mapping.Add("zip", "application/zip");
			_mapping.Add("gz", "application/x-gzip");
			_mapping.Add("gzip", "application/x-gzip");
			_mapping.Add("m4a", "audio/mp4");
			_mapping.Add("gif", "image/gif");
			_mapping.Add("jpg", "image/jpeg");
			_mapping.Add("jpeg", "image/jpeg");
			_mapping.Add("png", "image/png");
			_mapping.Add("svg", "image/svg+xml");
			_mapping.Add("tif", "image/tiff");
			_mapping.Add("tiff", "image/tiff");
			_mapping.Add("csv", "text/csv");
			_mapping.Add("html", "text/html");
			_mapping.Add("mpg", "video/mpeg");
			_mapping.Add("mpeg", "video/mpeg");
			_mapping.Add("mp4", "video/mp4");
			_mapping.Add("ogg", "video/ogg");
			_mapping.Add("qt", "video/quicktime");
			_mapping.Add("mov", "video/quicktime");
	
		}

		public static string ResolveContentType(string fileName)
		{
			if(fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			var result = "application/octet-stream";
			var ext = Path.GetExtension(fileName).Substring(1).ToLower();
			if (ext != null)
			{
				_mapping.TryGetValue(ext, out result);
			}
			
			return result;
		}
	}
}
