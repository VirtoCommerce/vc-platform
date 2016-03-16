using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class ShoppingCart : AuditableEntity, IHaveTaxDetalization, IHasDynamicProperties
    {
		public string Name { get; set; }
		public string StoreId { get; set; }
		public string ChannelId { get; set; }
		public bool IsAnonymous { get; set; }
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string OrganizationId { get; set; }
		public string Currency { get; set; }
	
		public string LanguageCode { get; set; }
		public bool? TaxIncluded { get; set; }
		public bool? IsRecuring { get; set; }
		public string Comment { get; set; }

		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

        /// <summary>
        /// Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront 
        /// to select appropriate validation strategy
        /// </summary>
        public string ValidationType { get; set; }

        public decimal? VolumetricWeight { get; set; }

		public decimal Total { get; set; }
		public decimal SubTotal { get; set; }
		public decimal ShippingTotal { get; set; }
		public decimal HandlingTotal { get; set; }
		public decimal DiscountTotal { get; set; }
		public decimal TaxTotal { get; set; }

		public ICollection<Address> Addresses { get; set; }
		public ICollection<LineItem> Items { get; set; }
		public ICollection<Payment> Payments { get; set; }
		public ICollection<Shipment> Shipments { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public string Coupon { get; set; }

		#region IHaveTaxDetalization Members
		public ICollection<TaxDetail> TaxDetails { get; set; }
        #endregion

        #region IHasDynamicProperties Members
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

        #endregion
    }
}
