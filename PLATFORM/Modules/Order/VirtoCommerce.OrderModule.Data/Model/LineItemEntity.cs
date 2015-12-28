﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class LineItemEntity : AuditableEntity
	{
		public LineItemEntity()
		{
			Discounts = new NullCollection<DiscountEntity>();
			TaxDetails = new NullCollection<TaxDetailEntity>();
		}

		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		[Column(TypeName = "Money")]
		public decimal BasePrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal Price { get; set; }
		[Column(TypeName = "Money")]
		public decimal DiscountAmount { get; set; }
		[Column(TypeName = "Money")]
		public decimal Tax { get; set; }
		public int Quantity { get; set; }
		[Required]
		[StringLength(64)]
		public string ProductId { get; set; }
		[Required]
		[StringLength(64)]
		public string CatalogId { get; set; }
	
		[StringLength(64)]
		public string CategoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string Sku { get; set; }

        [StringLength(64)]
        public string ProductType { get; set; }
        [Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(2048)]
		public string Comment { get; set; }

		public bool IsReccuring { get; set; }

		[StringLength(1028)]
		public string ImageUrl { get; set; }
		public bool IsGift { get; set; }
		[StringLength(64)]
		public string ShippingMethodCode { get; set; }
		[StringLength(64)]
		public string FulfilmentLocationCode { get; set; }

		[StringLength(32)]
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }
		[StringLength(32)]
		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		[StringLength(64)]
		public string TaxType { get; set; }

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		[StringLength(2048)]
		public string CancelReason { get; set; }

		public virtual ObservableCollection<DiscountEntity> Discounts { get; set; }

		public virtual ObservableCollection<TaxDetailEntity> TaxDetails { get; set; }

		public virtual CustomerOrderEntity CustomerOrder { get; set; }
		public string CustomerOrderId { get; set; }
	}
}
