using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	public class CatalogItemReward : PromotionReward
	{
		private string _CategoryId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CategoryId
		{
			get
			{
				return _CategoryId;
			}
			set
			{
				SetValue(ref _CategoryId, () => this.CategoryId, value);
			}
		}

		private string _ProductId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ProductId
		{
			get
			{
				return _ProductId;
			}
			set
			{
				SetValue(ref _ProductId, () => this.ProductId, value);
			}
		}


		private string _SkuId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string SkuId
		{
			get
			{
				return _SkuId;
			}
			set
			{
				SetValue(ref _SkuId, () => this.SkuId, value);
			}
		}

		private string _ExcludingCategories;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ExcludingCategories
		{
			get
			{
				return _ExcludingCategories;
			}
			set
			{
				SetValue(ref _ExcludingCategories, () => this.ExcludingCategories, value);
			}
		}

		private string _ExcludingProducts;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ExcludingProducts
		{
			get
			{
				return _ExcludingProducts;
			}
			set
			{
				SetValue(ref _ExcludingProducts, () => this.ExcludingProducts, value);
			}
		}

		private string _ExcludingSkus;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ExcludingSkus
		{
			get
			{
				return _ExcludingSkus;
			}
			set
			{
				SetValue(ref _ExcludingSkus, () => this.ExcludingSkus, value);
			}
		}


		private decimal _QuantityLimit;

		[DataMember]
		public decimal QuantityLimit
		{
			get
			{
				return _QuantityLimit;
			}
			set
			{
				SetValue(ref _QuantityLimit, () => this.QuantityLimit, value);
			}
		}

		private decimal _ItemsCountLimit;

		[DataMember]
		public decimal ItemsCountLimit
		{
			get
			{
				return _ItemsCountLimit;
			}
			set
			{
				SetValue(ref _ItemsCountLimit, () => this.ItemsCountLimit, value);
			}
		}
	}
}
