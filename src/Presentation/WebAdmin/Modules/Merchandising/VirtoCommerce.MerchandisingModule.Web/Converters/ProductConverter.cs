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
	public static class ProductConverter
	{
		public static webModel.CatalogItem ToWebModel(this moduleModel.CatalogProduct product, webModel.Product parentProduct = null)
		{
			webModel.CatalogItem retVal = new webModel.Product();
			if(parentProduct != null)
			{
				retVal = new webModel.ProductVariation();
			}
			retVal.InjectFrom(product);
			
			if (product.Assets != null)
			{
				retVal.Images = product.Assets.Select(x => x.ToWebModel()).ToArray();
			}

			if (product.Variations != null)
			{
				((webModel.Product)retVal).Variations = product.Variations.Select(x => x.ToWebModel((webModel.Product)retVal)).OfType<webModel.ProductVariation>().ToArray();
			}

			retVal.Properties = new webModel.PropertyDictionary();
			//Need add property for each meta info

			foreach (var propValue in product.PropertyValues)
			{
				retVal.Properties.Add(propValue.PropertyName, propValue.Value);
			}
			return retVal;
		}

		public static moduleModel.CatalogProduct ToModuleModel(this webModel.CatalogItem catalogItem)
		{
			var retVal = new moduleModel.CatalogProduct();
			retVal.InjectFrom(catalogItem);

			if (catalogItem.Images != null)
			{
				retVal.Assets = new List<moduleModel.ItemAsset>();
				bool isMain = true;
				foreach (var productImage in catalogItem.Images)
				{
					var image = productImage.ToModuleModel();
					image.Type = moduleModel.ItemAssetType.Image;
					image.Group = isMain ? "primaryimage" : "images";
					retVal.Assets.Add(image);
					isMain = false;
				}
			}

			if (catalogItem.Properties != null)
			{
				retVal.PropertyValues = new List<moduleModel.PropertyValue>();
				foreach (var keyValue in catalogItem.Properties)
				{
					var propValue = new moduleModel.PropertyValue
					{
						PropertyName = keyValue.Key,
						Value = keyValue.Value.ToString()
					};
				}
			}

		
			return retVal;
		}
	}
}
