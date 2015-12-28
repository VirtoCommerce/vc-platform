﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Cart.Model
{
    public class LineItem : AuditableEntity, IHaveTaxDetalization, IHasDynamicProperties
	{
		public string ProductId { get; set; }
		public CatalogProduct Product { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string Sku { get; set; }
        public string ProductType { get; set; }

		public string Name { get; set; }
		public int Quantity { get; set; }

		public string FulfillmentLocationCode { get; set; }
		public string ShipmentMethodCode { get; set; }
		public bool RequiredShipping { get; set; }
		public string ThumbnailImageUrl { get; set; }
		public string ImageUrl { get; set; }

		public bool IsGift { get; set; }
		public CurrencyCodes Currency { get; set; }

		public string LanguageCode { get; set; }

		public string Note { get; set; }

		public bool IsReccuring { get; set; }

		public string TaxType { get; set; }
		public bool TaxIncluded { get; set; }

		public decimal? VolumetricWeight { get; set; }


		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }


		public decimal ListPrice { get; set; }
		public decimal SalePrice { get; set; }
		public decimal PlacedPrice { get; set; }
		public decimal ExtendedPrice { get; set; }

		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }

		public ICollection<Discount> Discounts { get; set; }

		#region IHaveTaxDetalization Members
		public ICollection<TaxDetail> TaxDetails { get; set; }
		#endregion

        #region IHasDynamicProperties Members
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

        #endregion
	}
}
