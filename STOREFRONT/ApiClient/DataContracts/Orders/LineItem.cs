using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class LineItem
    {
        #region Public Properties

        public decimal BasePrice { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Currency { get; set; }
        public string CustomerOrderId { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<Discount> Discounts { get; set; }
        public string FulfilmentLocationCode { get; set; }
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsGift { get; set; }
        public bool IsReccuring { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public string ShipmentId { get; set; }
        public string ShippingMethodCode { get; set; }
        public decimal Tax { get; set; }

        #endregion
    }
}
