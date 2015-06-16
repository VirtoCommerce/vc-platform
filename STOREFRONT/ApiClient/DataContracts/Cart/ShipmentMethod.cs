#region

using System.Collections.Generic;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{

    #region

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

        public string OptionName { get; set; }

        public string OptionDescription { get; set; }

        #endregion
    }
}
