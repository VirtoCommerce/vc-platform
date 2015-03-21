using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ProductImage : ProductAssetBase
	{
		public ProductImage()
		{
			TypeId = "image";
		    Group = "images";
		}

	    public bool IsThumb
	    {
	        get { return Group.EndsWith(".thumb") || Url.Contains(".thumb."); }
	    }
	}
}
