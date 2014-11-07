using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ProductAsset : ProductAssetBase
	{
		public ProductAsset()
		{
			TypeId = "asset";
		}
		public string Name { get; set; }
		public string Size { get; set; }
		public string MimeType { get; set; }
	}
}
