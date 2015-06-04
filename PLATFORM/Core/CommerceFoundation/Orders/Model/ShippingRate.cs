using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract, Serializable]
    public class ShippingRate
    {
        /// <summary>
        /// Represents the shipping rate ID.
        /// </summary>
        public string Id;
        /// <summary>
        /// Represents the shipping rate price.
        /// </summary>
        public decimal Price;
        /// <summary>
        /// Represents the shipping rate name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Represents the shipping rate's currency code.
        /// </summary>
        public string CurrencyCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingRate"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="price">The price.</param>
        /// <param name="currencyCode">The currency code.</param>
        public ShippingRate(string id, string name, decimal price, string currencyCode)
        {
            Id = id;
            Price = price;
            Name = name;
            CurrencyCode = currencyCode;
        }
    }
}
