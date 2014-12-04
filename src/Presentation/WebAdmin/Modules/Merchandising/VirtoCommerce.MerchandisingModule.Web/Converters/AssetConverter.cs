using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web2.Model;

namespace VirtoCommerce.MerchandisingModule.Web2.Converters
{
	public static class AssetConverter
	{
		public static webModel.ItemImage ToWebModel(this moduleModel.ItemAsset assset)
		{
			var retVal = new webModel.ItemImage();
			retVal.InjectFrom(assset);
			retVal.Src = assset.Url;
			retVal.ThumbSrc = assset.Url;
			return retVal;
		}

		public static moduleModel.ItemAsset ToModuleModel(this webModel.ItemImage itemInage)
		{
			var retVal = new moduleModel.ItemAsset();
			retVal.InjectFrom(itemInage);
			if(String.IsNullOrEmpty(retVal.Group))
			{
				retVal.Group = "default";
			}
			return retVal;
		}


	}
}
