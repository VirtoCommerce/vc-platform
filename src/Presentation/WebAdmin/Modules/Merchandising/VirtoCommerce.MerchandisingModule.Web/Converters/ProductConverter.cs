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
	public static class ProductConverter
	{
		public static webModel.CatalogItem ToWebModel(this moduleModel.CatalogProduct product,  Uri assetBaseUri, webModel.Product parentProduct = null)
		{
			webModel.CatalogItem retVal = new webModel.Product();
			if(parentProduct != null)
			{
				retVal = new webModel.ProductVariation();
			}
			retVal.InjectFrom(product);
			
			if (product.Assets != null)
			{
				retVal.Images = product.Assets.Where(x => x.Type == moduleModel.ItemAssetType.Image).Select(x => x.ToWebModel(assetBaseUri)).ToArray();
			}

			if (product.Variations != null)
			{
				((webModel.Product)retVal).Variations = product.Variations.Select(x => x.ToWebModel(assetBaseUri, (webModel.Product)retVal)).OfType<webModel.ProductVariation>().ToArray();
			}

		    if (product.Reviews != null)
		    {
                retVal.EditorialReviews = product.Reviews.Select(x => new webModel.EditorialReview().InjectFrom(x)).Cast<webModel.EditorialReview>().ToArray();
		    }

		    if (product.Links != null)
		    {
		        retVal.Categories = product.Links.Select(x => x.CategoryId).ToArray();
		    }

			retVal.Properties = new webModel.PropertyDictionary();
			//Need add property for each meta info

			foreach (var propValueGroup in product.PropertyValues.GroupBy(x=>x.PropertyName))
			{
                retVal.Properties.Add(propValueGroup.Key, propValueGroup.Select(g=>g.Value));
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
