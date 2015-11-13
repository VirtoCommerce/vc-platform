﻿using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class ShoppingCart
    {
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string Name { get; set; }

        public string StoreId { get; set; }

        public string ChannelId { get; set; }

        public bool IsAnonymous { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string OrganizationId { get; set; }

        public string Currency { get; set; }

        public string Coupon { get; set; }

        public string LanguageCode { get; set; }

        public bool TaxIncluded { get; set; }

        public bool IsRecuring { get; set; }

        public string Comment { get; set; }

        public decimal? VolumetricWeight { get; set; }

        public string WeightUnit { get; set; }

        public decimal? Weight { get; set; }

        public string MeasureUnit { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ShippingTotal { get; set; }

        public decimal HandlingTotal { get; set; }

        public decimal DiscountTotal { get; set; }

        public decimal TaxTotal { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<CartItem> Items { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public ICollection<Shipment> Shipments { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public ICollection<TaxDetail> TaxDetails { get; set; }
    }
}