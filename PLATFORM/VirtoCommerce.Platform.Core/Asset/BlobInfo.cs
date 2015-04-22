using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
{
	public class BlobInfo
	{
		public string Key { get; set; }
		public string FileName { get; set; }
		public long Size { get; set; }
		public string ContentType { get; set; }

	}
}
