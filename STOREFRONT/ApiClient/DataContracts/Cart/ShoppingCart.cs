#region

using System;
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{

    #region

    #endregion

    public class ShoppingCart
    {
        #region Public Properties

        public ICollection<Address> Addresses { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public Dimension Dimension { get; set; }

        public decimal DiscountTotal { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public decimal HandlingTotal { get; set; }

        public string Id { get; set; }

        public bool IsAnonymous { get; set; }

        public bool? IsRecuring { get; set; }

        public ICollection<CartItem> Items { get; set; }

        public string LanguageCode { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string OrganizationId { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public ICollection<Shipment> Shipments { get; set; }

        public decimal ShippingTotal { get; set; }

        public string StoreId { get; set; }

        public decimal SubTotal { get; set; }

        public bool? TaxIncluded { get; set; }

        public decimal TaxTotal { get; set; }

        public decimal Total { get; set; }

        public decimal? VolumetricWeight { get; set; }

        public Weight Weight { get; set; }

        #endregion
    }
}
