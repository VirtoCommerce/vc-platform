using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model
{
    public class ShippingMethod : ValueObject<ShippingMethod>
    {
        public ShippingMethod()
        {
            Discounts = new List<Discount>();
        }
        /// <summary>
        /// Gets or sets the value of shipping method code
        /// </summary>
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option name
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option description
        /// </summary>
        public string OptionDescription { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method logo absolute URL
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method tax type
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method price
        /// </summary>
        public Money Price { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping method discounts
        /// </summary>
        /// <value>
        /// Collection of Discount objects
        /// </value>
        public ICollection<Discount> Discounts { get; set; }
    }
}