using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Image asset
    /// </summary>
	public class Image : AssetBase
	{
		public Image()
		{
			TypeId = "image";
		    Group = "images";
		}

	}
}
