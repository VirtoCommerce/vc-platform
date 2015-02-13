namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    #region

    using System.Collections.Generic;

    #endregion

    public class ShipmentMethod
    {
        #region Public Properties

        public string Currency { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public string LogoUrl { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ShipmentMethodCode { get; set; }

        #endregion
    }
}