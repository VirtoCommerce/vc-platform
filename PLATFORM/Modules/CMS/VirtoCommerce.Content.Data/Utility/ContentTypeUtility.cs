using System;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Content.Data.Utility
{
	public static class ContentTypeUtility
	{
		public static string GetContentType(string path, byte[] content)
		{
			var retVal = DefaultContentType;

			var types = TypeInfos.Where(t => t.IsThisContentType(path, content));

			if (types.Any())
			{
				retVal = types.First().ContentType;
			}

			return retVal;
		}

		public static string ConvertImageToBase64String(byte[] image, string contentType)
		{
			var retVal = string.Format("{0}{1}",
				string.Format(Base64StringPrefix, contentType),
				Convert.ToBase64String(image));

			return retVal;
		}

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

		private static string Base64StringPrefix = "data:{0};base64,";
		private static string DefaultContentType = "application/octet-stream";

		private static ContentTypeInfo[] TypeInfos = new ContentTypeInfo[] 
		{
			new ContentTypeInfo { StartBytes = new byte[] { 66, 77 }, Extension = ".bmp", ContentType = "image/bmp", NumberOfBytes = 2 },
			new ContentTypeInfo { StartBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 }, Extension = ".png", ContentType = "image/png", NumberOfBytes = 16 },
			new ContentTypeInfo { StartBytes = new byte[] { 71, 73, 70, 56 }, Extension = ".gif", ContentType = "image/gif", NumberOfBytes = 4 },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".liquid", ContentType = "text/html" },
			new ContentTypeInfo { StartBytes = new byte[] { 0, 0, 1, 0 }, Extension = ".ico", ContentType = "image/x-icon", NumberOfBytes = 4 },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".js", ContentType = "application/javascript" },
			new ContentTypeInfo { StartBytes = new byte[] { 255, 216, 255 }, Extension = ".jpg", ContentType = "image/jpeg", NumberOfBytes = 3 },
			new ContentTypeInfo { StartBytes = new byte[] { 255, 216, 255 }, Extension = ".jpeg", ContentType = "image/jpeg", NumberOfBytes = 3 },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".eot", ContentType = "application/vnd.ms-fontobject"},
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".json", ContentType = "application/json" },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".svg", ContentType = "image/svg+xml" },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".ttf", ContentType = "application/x-font-truetype" },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".woff", ContentType = "application/font-woff" },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".html", ContentType = "text/html" },
			new ContentTypeInfo { StartBytes = new byte[] { }, Extension = ".md", ContentType = "text/html" },
		};
	}

	public struct ContentTypeInfo
	{
		public byte[] StartBytes { get; set; }
		public string Extension { get; set; }
		public string ContentType { get; set; }
		public int NumberOfBytes { get; set; }

		public bool IsThisContentType(string path, byte[] file)
		{
			var retVal = false;

			if (this.NumberOfBytes > 0 && file != null)
			{
				if (file.Take(this.NumberOfBytes).SequenceEqual(this.StartBytes))
				{
					retVal = true;
				}
				else
				{
					var extension = Path.GetExtension(path);
					if (this.Extension.ToLower().Equals(extension))
					{
						retVal = true;
					}
				}
			}
			else
			{
				var extension = Path.GetExtension(path);
				if (this.Extension.Equals(extension))
				{
					retVal = true;
				}
			}

			return retVal;
		}
	}
}



//		private static readonly byte[] BMP = { 66, 77 };
//		private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
//		private static readonly byte[] EXE_DLL = { 77, 90 };
//		private static readonly byte[] GIF = { 71, 73, 70, 56 };
//		private static readonly byte[] ICO = { 0, 0, 1, 0 };
//		private static readonly byte[] JPG = { 255, 216, 255 };
//		private static readonly byte[] MP3 = { 255, 251, 48 };
//		private static readonly byte[] OGG = { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 };
//		private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
//		private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
//		private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };
//		private static readonly byte[] SWF = { 70, 87, 83 };
//		private static readonly byte[] TIFF = { 73, 73, 42, 0 };
//		private static readonly byte[] TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
//		private static readonly byte[] TTF = { 0, 1, 0, 0, 0 };
//		private static readonly byte[] WAV_AVI = { 82, 73, 70, 70 };
//		private static readonly byte[] WMV_WMA = { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 };
//		private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };
