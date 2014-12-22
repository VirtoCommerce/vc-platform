using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Assets.Model
{
	public class BlobInfo
	{
		public string Name { get; set; }
		public long Size { get; set; }
		public string ContentType { get; set; }
		public string Location { get; set; }
	}
}
