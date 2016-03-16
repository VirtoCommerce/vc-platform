using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class ShoppingCart : AuditableEntity, IHaveTaxDetalization
    {
        /// <summary>
        /// Gets or sets the value of shopping cart name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of channel id
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is anonymous
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart organization id
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart coupon
        /// </summary>
        /// <value>
        /// Coupon object
        /// </value>
        public string Coupon { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart language code
        /// </summary>
        /// <value>
        /// Culture name in ISO 3166-1 alpha-3 format
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart has tax
        /// </summary>
        public bool TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is recurring
        /// </summary>
        public bool IsRecuring { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart text comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the value of volumetric weight
        /// </summary>
        public decimal? VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of weight unit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of height
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Gets or sets the value of length
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Gets or sets the value of width
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Represent any line item validation type (noPriceValidate, noQuantityValidate etc) this value can be used in storefront 
        /// to select appropriate validation strategy
        /// </summary>
        public string ValidationType { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total cost
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart subtotal
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping total cost
        /// </summary>
        public decimal ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of handling total cost
        /// </summary>
        public decimal HandlingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total discount amount
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total tax cost
        /// </summary>
        public decimal TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart addresses
        /// </summary>
        /// <value>
        /// Collection of Address objects
        /// </value>
        public ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart line items
        /// </summary>
        /// <value>
        /// Collection of LineItem objects
        /// </value>
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart payments
        /// </summary>
        /// <value>
        /// Collection of Payment objects
        /// </value>
        public ICollection<Payment> Payments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart shipments
        /// </summary>
        /// <value>
        /// Collection of Shipment objects
        /// </value>
        public ICollection<Shipment> Shipments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart discounts
        /// </summary>
        /// <value>
        /// Collection of Discount objects
        /// </value>
        public ICollection<Discount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>
        /// Collection of TaxDetail objects
        /// </value>
        public ICollection<TaxDetail> TaxDetails { get; set; }

        /// <summary>
        /// Used for dynamic properties management, contains object type string
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// Dynamic properties collections
        /// </summary>
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

    }
}