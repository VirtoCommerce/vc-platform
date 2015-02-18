#region

using System.Collections.Generic;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{

    #region

    #endregion

    public class CartItem
    {
        #region Public Properties

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Currency { get; set; }

        public Dimension Dimension { get; set; }

        public decimal DiscountTotal { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public decimal ExtendedPrice { get; set; }

        public bool? Gift { get; set; }

        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public bool? IsReccuring { get; set; }

        public string LanguageCode { get; set; }

        public decimal ListPrice { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public decimal PlacedPrice { get; set; }

        public string ProductCode { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public bool? RequiredShipping { get; set; }

        public decimal SalePrice { get; set; }

        public string ShipmentMethodCode { get; set; }

        public bool? TaxIncluded { get; set; }

        public decimal TaxTotal { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public decimal? VolumetricWeight { get; set; }

        public string WarehouseLocation { get; set; }

        public Weight Weight { get; set; }

        #endregion
    }
}
