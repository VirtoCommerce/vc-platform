﻿using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Cart
{
    public class Shipment : Entity
    {
        public Shipment()
        {
            Discounts = new List<Discount>();
            Items = new List<LineItem>();
            TaxDetails = new List<TaxDetail>();
        }

        /// <summary>
        /// Gets or sets the value of shipping method code
        /// </summary>
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option
        /// </summary>
        public string ShipmentMethodOption { get; set; }

        /// <summary>
        /// Gets or sets the value of fulfillment center id
        /// </summary>
        public string FulfilmentCenterId { get; set; }

        /// <summary>
        /// Gets or sets the delivery address
        /// </summary>
        /// <value>
        /// Address object
        /// </value>
        public Address DeliveryAddress { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of volumetric weight
        /// </summary>
        public decimal? VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of weight unit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement units
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
        /// Gets or sets the flag of shipping has tax
        /// </summary>
        public bool TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping price
        /// </summary>
        public Money ShippingPrice { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping price
        /// </summary>
        public Money Total { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping discount amount
        /// </summary>
        public Money DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total shipping tax amount
        /// </summary>
        public Money TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping items subtotal
        /// </summary>
        public Money ItemSubtotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping subtotal
        /// </summary>
        public Money Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping discounts
        /// </summary>
        /// <value>
        /// Collection of Discount objects
        /// </value>
        public ICollection<Discount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping items
        /// </summary>
        /// <value>
        /// Collection of LineItem objects
        /// </value>
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping tax type
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>
        /// Collection of TaxDetail objects
        /// </value>
        public ICollection<TaxDetail> TaxDetails { get; set; }
    }
}