using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.OrderModule.Web.Model
{
    public class ShippingMethod : ValueObject<ShippingMethod>
    {
        /// <summary>
        /// Code used for link shipment with external carrier service implementation (FedEx, USPS etc)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Method name (system name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Describe some shipment options (Vip, Air, Moment etc)
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

    }
}