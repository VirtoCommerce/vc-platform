using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class ShippingMethod : ValueObject<ShippingMethod>
    {
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
        /// Gets or sets the value of shipping method currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping method discounts
        /// </summary>
        /// <value>
        /// Collection of Discount objects
        /// </value>
        public ICollection<Discount> Discounts { get; set; }

        public ICollection<SettingEntry> Settings { get; set; }
    }
}