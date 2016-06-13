using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Web.Model.Asset
{
	public class BlobInfo
	{
		public string RelativeUrl { get; set; }
		public string Url { get; set; }
		public string Name { get; set; }
		public string Size { get; set; }
		public string MimeType { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
