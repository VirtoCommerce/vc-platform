using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class AssetConverter
	{
		public static webModel.ItemImage ToWebModel(this moduleModel.ItemAsset assset, Uri baseUri)
		{
			var retVal = new webModel.ItemImage();
			retVal.InjectFrom(assset);
			retVal.Src = new Uri(baseUri, assset.Url).ToString();
			retVal.ThumbSrc = new Uri(baseUri, assset.Url).ToString();
			retVal.Name = assset.Group;
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
