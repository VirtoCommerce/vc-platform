using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Inventory.Model;
using System.Globalization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	public sealed class CsvProduct : CatalogProduct
	{
		private readonly IBlobUrlResolver _blobUrlResolver;
		public CsvProduct()
		{
			SeoInfos = new List<SeoInfo>();
			Reviews = new List<EditorialReview>();
			PropertyValues = new List<PropertyValue>();
			Images = new List<Image>();
			Assets = new List<Asset>();


			Price = new Price() { Currency = CurrencyCodes.USD };
			Inventory = new InventoryInfo();
			EditorialReview = new EditorialReview();
			Reviews = new List<EditorialReview>();
			Reviews.Add(EditorialReview);
			SeoInfo = new SeoInfo();
			SeoInfos = new List<SeoInfo>();
			SeoInfos.Add(SeoInfo);
		}

		public CsvProduct(CatalogProduct product, IBlobUrlResolver blobUrlResolver, Price price, InventoryInfo inventory)
			: this()
		{
			_blobUrlResolver = blobUrlResolver;

			this.InjectFrom(product);
			PropertyValues = product.PropertyValues;
			Images = product.Images;
			Assets = product.Assets;
			Links = product.Links;
			Variations = product.Variations;
			SeoInfos = product.SeoInfos;
			Reviews = product.Reviews;
			Associations = product.Associations;
			if (price != null)
			{
				Price = price;
			}
			if (inventory != null)
			{
				Inventory = inventory;
			}
		
		}
		public Price Price { get; set; }
		public InventoryInfo Inventory { get; set; }
		public EditorialReview EditorialReview { get; set; }
		public SeoInfo SeoInfo { get; set; }

		public string PriceId
		{
			get
			{
				return Price.Id;
			}
			set
			{
				Price.Id = value;
			}
		}
		public string SalePrice
		{
			get
			{
				return Price.Sale != null ? Price.Sale.Value.ToString(CultureInfo.InvariantCulture) : null;
			}
			set
			{
				if (!String.IsNullOrEmpty(value))
				{
					Price.Sale = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
				}
			}
		}

		public string ListPrice
		{
			get
			{
				return Price.List.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				Price.List = String.IsNullOrEmpty(value) ? 0 : Convert.ToDecimal(value, CultureInfo.InvariantCulture);
			}
		}
		
		public string Currency
		{
			get
			{
				return Price.Currency.ToString();
			}
			set
			{
				Price.Currency = EnumUtility.SafeParse<CurrencyCodes>(value, CurrencyCodes.USD);
			}
		}

		public string FulfillmentCenterId
		{
			get
			{
				return Inventory.FulfillmentCenterId;
			}
			set
			{
				Inventory.FulfillmentCenterId = value;
			}
		}

		public string Quantity
		{
			get
			{
				return Inventory.InStockQuantity.ToString();
			}
			set
			{
				Inventory.InStockQuantity = Convert.ToInt64(value);
			}
		}

		public string PrimaryImage
		{
			get
			{
				var retVal = String.Empty;
				if (Images != null)
				{
					var primaryImage = Images.OrderBy(x => x.SortOrder).FirstOrDefault();
					if (primaryImage != null)
					{
						retVal = _blobUrlResolver != null ? _blobUrlResolver.GetAbsoluteUrl(primaryImage.Url) : primaryImage.Url;
					}
				}
				return retVal;
			}

			set
			{
				if (!String.IsNullOrEmpty(value))
				{
					Images.Add(new Image
					{
						Url = value,
						SortOrder = 0
					});
				}
			}
		}

		public string AltImage
		{
			get
			{
				var retVal = String.Empty;
				if (Images != null)
				{
					var primaryImage = Images.OrderBy(x => x.SortOrder).Skip(1).FirstOrDefault();
					if (primaryImage != null)
					{
						retVal = _blobUrlResolver != null ? _blobUrlResolver.GetAbsoluteUrl(primaryImage.Url) : primaryImage.Url;
					}
				}
				return retVal;
			}

			set
			{
				if (!String.IsNullOrEmpty(value))
				{
					Images.Add(new Image
					{
						Url = value,
						SortOrder = 1
					});
				}
			}
		}
		public string Sku
		{
			get { return Code; }
			set { Code = value; }
		}

		public string ParentSku { get; set; }

		public string CategoryPath
		{
			get
			{
				return Category != null ? String.Join("/", Category.Parents.Select(x => x.Name).Concat(new string[] { Category.Name })) : null;
			}
			set
			{
				Category = new Category { Path = value };
			}
		}

		public string Review
		{
			get { return EditorialReview.Content; }
			set { EditorialReview.Content =  value; }
		}

		public string SeoTitle
		{
			get	{ return SeoInfo.PageTitle; }
			set { SeoInfo.PageTitle = value; }
		}

		public string SeoUrl
		{
			get { return SeoInfo.SemanticUrl; }
			set
            {
                var slug = value.GenerateSlug();
                SeoInfo.SemanticUrl = slug.Substring(0, Math.Min(slug.Length, 240));
            }
		}

		public string SeoDescription
		{
			get { return SeoInfo.MetaDescription; }
			set { SeoInfo.MetaDescription = value; }
		}

	
	
	}
}